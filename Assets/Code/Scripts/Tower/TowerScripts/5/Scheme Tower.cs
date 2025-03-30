using System.Collections;
using UnityEngine;

public class SchemeTower : BaseTower
{
    private int tailRecursionStack = 0;
    private const int maxStack = 10; // Maximum stacking limit (caps at 3x damage)
    private const float stackBonus = 0.2f; // 20% increased damage per stack

    public SchemeTower()
    {
        towerClassification = Classifaction.FunctionalLogicBased;
    }

    public override void Shoot()
    {
        if (target == null) return;

        float modifiedDamage = damage * (1 + tailRecursionStack * stackBonus);
        FireBullet(target, modifiedDamage);

        tailRecursionStack = Mathf.Min(tailRecursionStack + 1, maxStack);
        StopCoroutine(nameof(DecayStack));
        StartCoroutine(DecayStack());
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

    private IEnumerator DecayStack()
    {
        yield return new WaitForSeconds(2f); // Start decay after 2 seconds of no attacks
        while (tailRecursionStack > 0)
        {
            tailRecursionStack--;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
