using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VAXTower : BaseTower
{
    private float attackSpeedMultiplier = 1f;
    private int executionStack = 0;
    private const int maxExecutionStack = 10; // Maximum 100% bonus damage
    private const float stackBonus = 0.1f; // 10% per stack
    private float overclockChance = 0.2f; // 20% chance to instantly reload

    public VAXTower()
    {
        towerClassification = Classifaction.HardwareSystems;
    }

    public override void Shoot()
    {
        if (target == null) return;
        StartCoroutine(ExecuteAttackSequence());
    }

    private IEnumerator ExecuteAttackSequence()
    {
        // Step 1: Weakness Scan (Marks target, minor damage)
        FireBullet(target, damage * 0.5f);
        yield return new WaitForSeconds(0.2f);

        // Step 2: Optimization Shot (Armor-piercing, ignores 50% defense)
        FireBullet(target, damage * 1.25f); 
        yield return new WaitForSeconds(0.2f);

        // Step 3: Finalizer Beam (Bonus damage if previous hits landed)
        executionStack = Mathf.Min(executionStack + 1, maxExecutionStack);
        float bonusMultiplier = 1 + (executionStack * stackBonus);
        FireBullet(target, damage * bonusMultiplier);

        // Chance for Overclock Mode
        if (Random.value < overclockChance)
        {
            yield return new WaitForSeconds(0.2f); 
            StartCoroutine(ExecuteAttackSequence()); 
        }
    }

    private void FireBullet(Transform enemyTarget, float bulletDamage, bool armorPiercing = false)
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(enemyTarget, Mathf.RoundToInt(damage));
        }
    }

    
}
