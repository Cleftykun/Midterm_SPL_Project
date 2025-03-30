using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForthTower : BaseTower
{
    [Header("Burst Fire Settings")]
    public int burstCount = 3; // Number of bullets per burst
    public float burstDelay = 0.1f; // Time delay between each bullet

    private float modifiedAttackSpeed; // Adjusted attack speed

    public ForthTower()
    {
        towerClassification = Classifaction.AlgorithmicTheoreticalCS;
    }

    protected override void Start()
    {
        base.Start();
        modifiedAttackSpeed = baseAttackSpeed * 0.75f; // Slightly slower attack speed
    }

    public override void Shoot()
    {
        if (target == null) return;
        StartCoroutine(BurstFire());
    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < burstCount; i++)
        {
            FireBullet(target, damage);
            yield return new WaitForSeconds(burstDelay);
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
