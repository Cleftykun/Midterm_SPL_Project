using UnityEngine;
using System.Collections;

public class LogicGate : MonoBehaviour
{
    [Header("Logic Gate Settings")]
    [SerializeField] private Rigidbody2D rb;
    private float duration;
    private float weakenEffect;
    private int triggerCount = 5;
    private float moveSpeed = 5f;
    private Transform target;
    private bool hasReachedTarget = false;

    public void Initialize(float effect, float gateLifetime)
    {
        weakenEffect = effect;
        duration = gateLifetime;
        StartCoroutine(GateLifetime());
    }

    private void Update()
    {
        if (!hasReachedTarget && target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                hasReachedTarget = true;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private IEnumerator GateLifetime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ApplySlow(weakenEffect, 3f); // Reduce speed by weakenEffect%
            triggerCount--;
        }

        if (triggerCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
