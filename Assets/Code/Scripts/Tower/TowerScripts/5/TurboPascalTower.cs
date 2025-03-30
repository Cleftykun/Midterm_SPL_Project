using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboPascalTower : BaseTower
{
    private int burstCount = 3; // Fires 3 bullets per attack
    private float burstInterval = 0.1f; // Time between each bullet in the burst

    public TurboPascalTower()
    {
        towerClassification = Classifaction.StructuredProcedural;
    }

    public override void Shoot()
    {
        if (target == null) return;
        StartCoroutine(FireBurst());
    }

    private IEnumerator FireBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            FireBullet(target, damage);
            yield return new WaitForSeconds(burstInterval);
        }
    }

    private void FireBullet(Transform enemyTarget, float bulletDamage)
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(enemyTarget, Mathf.RoundToInt(bulletDamage));
        }
    }
}
