using UnityEngine;

public class AISentry : MonoBehaviour
{
    public float speed = 5f;
    public float detectionRange = 10f; // Range to detect enemies
    public float attackRange = 5f; // Range to shoot at enemies
    public float fireCooldown = 1f; // Time between shots (cooldown)
    public GameObject bulletPrefab; // Bullet prefab reference
    public Transform firingPoint; // Position from where the bullets will spawn

    private Enemy target;
    private Rigidbody2D rb;
    private int damage = 10; // Sentry's attack damage (you can adjust this)

    private float fireTimer = 0f; // Timer to track cooldown

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("AISentry is missing a Rigidbody2D!");
            return;
        }

        // Set rigidbody to Kinematic to prevent physics from affecting the sentry
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        // Find a target initially
        FindTarget();
    }

    private void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindTarget();
            return; // Exit if no target is found
        }

        // Move manually toward the target
        Vector2 direction = (target.transform.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // If the enemy is within range to shoot, shoot at it
        if (Vector2.Distance(transform.position, target.transform.position) <= attackRange)
        {
            // Only shoot if the fire cooldown has elapsed
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireCooldown)
            {
                ShootAtTarget();
                fireTimer = 0f; // Reset fire cooldown timer
            }
        }
    }

    private void ShootAtTarget()
    {
        // Shoot at the target only if it's within the attack range
        if (target != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

            // Initialize the bullet with the target and damage
            bulletScript.Initialize(target.transform, damage);
        }
    }

    private void FindTarget()
    {
        // Find the closest enemy within detection range
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        Enemy closestEnemy = null;

        // Loop through all enemies within range and choose a random one
        foreach (var collider in enemiesInRange)
        {
            Enemy possibleTarget = collider.GetComponent<Enemy>();
            if (possibleTarget != null && possibleTarget.gameObject.activeInHierarchy)
            {
                // Find random enemy
                if (Random.Range(0, 2) == 0) // Randomly choose to target this enemy
                {
                    target = possibleTarget;
                    break;
                }
            }
        }

        // If no random target was selected, default to the closest enemy
        if (target == null)
        {
            target = closestEnemy;
        }
    }
}
