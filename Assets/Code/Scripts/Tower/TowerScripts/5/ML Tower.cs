using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLTower : BaseTower
{
    [Header("ML Tower Settings")]
    [SerializeField] private float learningRate = 0.1f; // Rate at which damage increases per attack
    [SerializeField] private float maxAdaptation = 1.5f; // Max damage increase (150%)
    [SerializeField] private float adaptationDecay = 0.02f; // Slowly loses adaptation over time
    private Dictionary<string, float> damageBoosts = new Dictionary<string, float>(); // Stores buffs per enemy type

    protected override void Start()
    {
        base.Start();
        InitializeAdaptation();
    }

    public override void Shoot()
    {
        if (target == null) return;

        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy == null) return;

        float adaptedDamage = GetAdaptedDamage(enemy);
        FireBullet(target, Mathf.RoundToInt(adaptedDamage));
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

    private void InitializeAdaptation()
    {
        // Set default damage multiplier for all enemy types
        damageBoosts.Clear();
    }

    private float GetAdaptedDamage(Enemy enemy)
    {
        string enemyType = enemy.enemyName;

        // If this enemy type hasn't been encountered, initialize it
        if (!damageBoosts.ContainsKey(enemyType))
        {
            damageBoosts[enemyType] = 1.0f;
        }

        // Increase adaptation for this specific enemy type
        damageBoosts[enemyType] += learningRate;
        damageBoosts[enemyType] = Mathf.Min(damageBoosts[enemyType], maxAdaptation);

        return damage * damageBoosts[enemyType];
    }

    private void Update()
    {
        base.Update();

        // Apply decay over time (prevents permanent adaptation)
        List<string> keys = new List<string>(damageBoosts.Keys);
        foreach (string key in keys)
        {
            damageBoosts[key] = Mathf.Max(damageBoosts[key] - adaptationDecay * Time.deltaTime, 1.0f);
        }
    }
}
