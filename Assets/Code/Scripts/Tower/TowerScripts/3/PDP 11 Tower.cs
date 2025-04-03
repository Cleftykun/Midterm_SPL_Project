using System.Collections;
using UnityEngine;

public class PDP11Tower : BaseTower
{
    public float burstFireRate = 0.2f; // Time between each bullet in the burst
    public float damageMultiplier = 1.5f; // Damage multiplier for subsequent bullets

    public override void Shoot()
    {
        if (target == null) return;
        StartCoroutine(ExecuteBurstAttack());
    }

    private IEnumerator ExecuteBurstAttack()
    {
        // Fire the first bullet with the normal damage
        FireBullet(target, damage);
        yield return new WaitForSeconds(burstFireRate);

        // Fire the second bullet with increased damage
        FireBullet(target, damage * damageMultiplier);
        yield return new WaitForSeconds(burstFireRate);

        // Fire the third bullet with even higher damage
        FireBullet(target, damage * damageMultiplier * damageMultiplier);
    }

    private void FireBullet(Transform enemyTarget, float bulletDamage)
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(enemyTarget, Mathf.RoundToInt(bulletDamage)); // Pass the calculated damage
        }
    }

    protected override void FindTarget()
    {
        base.FindTarget(); // Standard target finding behavior
    }
}
