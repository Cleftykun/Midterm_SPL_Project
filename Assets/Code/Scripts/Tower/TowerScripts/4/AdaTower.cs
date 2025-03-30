using UnityEngine;
using System.Collections;

public class AdaTower : BaseTower
{
    private float attackSpeedIncrease = 0.2f;
    private float speedResetTime = 10f;

    public AdaTower()
    {
        towerClassification = Classifaction.StructuredProcedural;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(IncreaseAttackSpeedOverTime());
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

    protected override void FindTarget()
    {
        base.FindTarget();
    }

    private IEnumerator IncreaseAttackSpeedOverTime()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < speedResetTime)
            {
                currentAttackSpeed += attackSpeedIncrease * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            currentAttackSpeed = baseAttackSpeed; // Reset attack speed after 10 seconds
        }
    }
}
