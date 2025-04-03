using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbaraLiskovTower : BaseTower
{
    [Header("Liskov Tower Settings")]
    [SerializeField] private float multiShotChance = 0.3f; // 30% chance for additional bullets
    [SerializeField] private int maxExtraBullets = 2; // Fires up to 2 extra bullets
    [SerializeField] private float extraBulletDamageMultiplier = 0.5f; // Extra bullets deal 50% damage

    [Header("Buff Settings")]
    [SerializeField] private float buffRange = 4f; // Range to buff other OOP towers
    [SerializeField] private float attackSpeedBoost = 1.25f; // 25% faster attack speed for OOP towers
    [SerializeField] private float extraMultiShotChance = 0.15f; // Adds 15% more multi-shot chance

    private List<BaseTower> buffedTowers = new List<BaseTower>();

    protected override void Start()
    {
        base.Start();
        ApplyBuffToOOPTowers();
    }

    public override void Shoot()
    {
        if (target == null) return;

        // Always fire the primary bullet
        FireBullet(target, damage);

        // Try additional multi-shot bullets
        StartCoroutine(TryFireExtraBullets());
    }

    private IEnumerator TryFireExtraBullets()
    {
        int bulletsFired = 0;

        while (bulletsFired < maxExtraBullets && Random.value < multiShotChance)
        {
            yield return new WaitForSeconds(0.1f); // Small delay between extra shots
            FireBullet(target, Mathf.RoundToInt(damage * extraBulletDamageMultiplier));
            bulletsFired++;
        }
    }

    private void FireBullet(Transform enemyTarget, int bulletDamage)
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(enemyTarget, bulletDamage);
        }
    }

    private void ApplyBuffToOOPTowers()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, buffRange);

        foreach (var hit in hits)
        {
            BaseTower tower = hit.GetComponent<BaseTower>();
            if (tower != null && tower.GetClassifaction() == Classifaction.ObjectOrientedModular)
            {
                buffedTowers.Add(tower);
                tower.SetAttackSpeed(tower.GetAttackSpeed() * attackSpeedBoost);

                if (tower is BarbaraLiskovTower liskovTower)
                {
                    liskovTower.multiShotChance += extraMultiShotChance; // Further boost multi-shot towers
                }
            }
        }
    }

    private void OnDestroy()
    {
        // Remove buffs when this tower is destroyed
        foreach (var tower in buffedTowers)
        {
            if (tower != null)
            {
                tower.SetAttackSpeed(tower.GetAttackSpeed() / attackSpeedBoost);
            }
        }
    }
}
