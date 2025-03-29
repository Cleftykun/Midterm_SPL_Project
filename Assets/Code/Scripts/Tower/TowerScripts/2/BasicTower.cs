using UnityEngine;
using System.Collections.Generic;

public class BasicTower : BaseTower
{
    private static int basicTowerCount = 0; 
    private const float damageBoostPerTower = 0.1f; 

    protected override void Start()
    {
        base.Start();
        basicTowerCount++;
        ApplyStackingDamageBoost();
    }

    private void ApplyStackingDamageBoost()
    {
        float boostFactor = 1f + (damageBoostPerTower * (basicTowerCount - 1));
        damage = Mathf.RoundToInt(originalDamage * boostFactor);
        Debug.Log($"{towerName} Damage Boost Applied: {damage} (Boost Factor: {boostFactor})");
    }

    public override void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(target, damage); 
        }
    }
}
