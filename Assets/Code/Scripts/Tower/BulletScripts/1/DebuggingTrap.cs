using UnityEngine;
using System.Collections;

public class DebuggingTrap : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;
    private float duration;
    private int trigger = 5;
    private float bulletSpeed = 10f;
    protected Transform target;
    private bool hasReachedTarget = false;
    public void Initialize(float trapDuration, Transform target)
    {
        duration = trapDuration;
        this.target = target;
        StartCoroutine(TrapLifetime());
        
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        }   
    }
    private void Update()
    {
        if (!hasReachedTarget && target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * Time.deltaTime);

            // Stop moving when close enough to the target
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                hasReachedTarget = true;
                rb.linearVelocity = Vector2.zero; // Ensure it stops moving
            }
        }
    }

    private IEnumerator TrapLifetime()
    {
        if(trigger <= 0){
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>(); // Fix here
        if (enemy != null)
        {
            enemy.Freeze(3);
            trigger--;
        }
    }


}
