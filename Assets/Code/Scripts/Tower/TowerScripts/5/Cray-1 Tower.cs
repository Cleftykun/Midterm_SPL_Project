using UnityEngine;
using System.Collections;

public class Cray1Tower : BaseTower
{
    [Header("Cray-1 Settings")]
    public float burstFireRate = 0.1f; // Time between each bullet in the burst (fast fire)
    public int burstSize = 3; // Number of bullets fired in one burst
    public float chargeTime = 2f; // Time to charge before the next burst
    public float damageMultiplier = 15f; // Damage scales with charge time
    private float currentCharge = 0f; // Current charge level
    private bool isCharging = false; // Is the tower charging for next burst
    private bool isFiring = false;

    protected override void Update()
    {
        if (target == null)
        {
            FindTarget(); // No target, find one
        }

        if (target != null)
        {
            RotateTowardsTarget(); // Rotate turret to target

            if (!isFiring && !isCharging)
            {
                StartCoroutine(FireBurst());
            }
        }
    }

    private IEnumerator FireBurst()
    {
        isFiring = true;
        for (int i = 0; i < burstSize; i++)
        {
            Shoot();
            yield return new WaitForSeconds(burstFireRate);
        }

        isFiring = false;
        isCharging = true;
        StartCoroutine(ChargeUp());
    }

    public override void Shoot()
    {
        if (bulletPrefab == null || firingPoint == null) return;

        // Calculate damage based on current charge
        float finalDamage = damage + currentCharge * damageMultiplier;

        // Fire a single bullet (for the burst)
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        ENIACBullet bullet = bulletObj.GetComponent<ENIACBullet>();
        if (bullet != null)
        {
            bullet.Initialize(target, (int)finalDamage); // Pass damage to bullet
        }
    }

    private IEnumerator ChargeUp()
    {
        currentCharge = 0f;
        while (currentCharge < chargeTime)
        {
            currentCharge += Time.deltaTime;
            yield return null;
        }
        isCharging = false;
    }

    protected override void RotateTowardsTarget()
    {
        if (target == null) return;

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    protected override void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentAttackRange, enemyMask);
        if (hits.Length == 0)
        {
            target = null;
            return;
        }

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                target = enemy.transform;
                return;
            }
        }

        target = null;
    }
}