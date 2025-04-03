using UnityEngine;

public class AINode : MonoBehaviour
{
    public float speed = 5f;
    public float detectionRange = 5f;
    public float lifetime = 10f;

    private Enemy target;
    private int damage;
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("AINode is missing a Rigidbody2D!");
            return;
        }

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        FindTarget();
        Destroy(gameObject, lifetime);
    }

    public void Initialize(int attackDamage)
    {
        damage = attackDamage;
        FindTarget(); // Find a target when spawned
    }

    private void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindTarget();
            if (target == null) return; // Stop if no target is found
        }

        // Move manually toward the target
        Vector2 direction = (target.transform.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void FindTarget()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        float shortestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (var collider in enemiesInRange)
        {
            Enemy possibleTarget = collider.GetComponent<Enemy>();
            if (possibleTarget != null && possibleTarget.gameObject.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, possibleTarget.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestEnemy = possibleTarget;
                }
            }
        }

        target = closestEnemy; // Set the closest enemy as the target
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy hitEnemy))
        {
            if (target == null || hitEnemy == target) // Ensure it only damages the correct target
            {
                ApplyDamage(hitEnemy);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyDamage(Enemy enemy)
    {
        if (enemy != null && enemy.gameObject.activeInHierarchy)
        {
            Debug.Log("AI Node: Damage Dealt " + damage);
            enemy.TakeDamage(damage);
        }
    }

   
}
