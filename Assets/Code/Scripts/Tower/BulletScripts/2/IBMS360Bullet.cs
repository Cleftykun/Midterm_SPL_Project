using UnityEngine;

public class IBMS360Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float explosionRadius = 2f;
    private Transform target;
    private int damage;
    private bool isOverclocked;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform target, int damage, bool overclocked = false)
    {
        this.target = target;
        this.damage = damage;
        this.isOverclocked = overclocked;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (target != null)
        {
            ApplyDamage(damage);
        }

        if (isOverclocked)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.transform.position, explosionRadius);
            foreach (Collider2D hit in hitEnemies)
            {
                Enemy nearbyEnemy = hit.GetComponent<Enemy>();
                if (nearbyEnemy != null)
                {
                    ApplyDamage(damage / 2);
                }
            }
        }

        Destroy(gameObject);
    }
    protected virtual void ApplyDamage(int damageTotal)
    {
        Enemy enemyHealth = target.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damageTotal);
        }
    }
}
