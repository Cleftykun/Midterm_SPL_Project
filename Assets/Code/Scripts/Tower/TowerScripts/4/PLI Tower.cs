using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLITower : BaseTower
{
    private float attackSpeedMultiplier = 1f;
    private float attackCooldown;
    private float baseAttackCooldown = 1f; // Set default attack speed

    public override void Shoot()
    {
        if (target == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null)
        {
            int enemyHP = target.GetComponent<Enemy>().hitpoints;

            if (enemyHP >= 50)
            {
                // High HP: Armor-piercing attack
                bulletScript.Initialize(target, Mathf.RoundToInt(damage * 1.5f));
                attackSpeedMultiplier = 1f; // Normal speed
            }
            else
            {
                // Low HP: Rapid weak attacks
                bulletScript.Initialize(target, Mathf.RoundToInt(damage * 0.5f));
                attackSpeedMultiplier = 2f; // Fire twice as fast
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        attackCooldown = baseAttackCooldown / attackSpeedMultiplier;
    }
}
