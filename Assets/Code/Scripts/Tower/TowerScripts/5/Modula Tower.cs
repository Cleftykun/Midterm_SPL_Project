using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModulaTower : BaseTower
{
    private int burstCount = 3; // Fires 3 bullets per attack
    private float burstInterval = 0.1f; // Time between each bullet in the burst
    private float nearbyStructuredBonus = 1.2f; // 20% increased fire rate if near Structured & Procedural towers

    public ModulaTower()
    {
        towerClassification = Classifaction.ObjectOrientedModular;
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

    protected override void Update()
    {
        base.Update();
        CheckForStructuredTowers();
    }

    private void CheckForStructuredTowers()
    {
        Collider2D[] nearbyTowers = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (var collider in nearbyTowers)
        {
            BaseTower tower = collider.GetComponent<BaseTower>();
            if (tower != null && tower.GetClassifaction() == Classifaction.StructuredProcedural)
            {
                currentAttackSpeed = baseAttackSpeed * nearbyStructuredBonus;
                return;
            }
        }
        currentAttackSpeed = baseAttackSpeed;
    }
}