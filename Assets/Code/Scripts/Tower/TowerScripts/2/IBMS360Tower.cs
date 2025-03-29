using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IBMS360Tower : BaseTower, IAbility
{
    [Header("IBM System/360 Buff Settings")]
    public float attackSpeedBoost = 0.2f; // +20% attack speed
    public float damageBoost = 0.15f; // +15% damage
    public static float globalAttackBoost = 0.1f; // +10% for all towers

    [Header("Ultimate Ability: System/360 Overclock")]
    public float overclockDuration = 10f;
    public float overclockMultiplier = 3f;
    public float explosionRadius = 2f;
    private bool isOverclockActive = false;

    private static List<IBMS360Tower> activeBuffTowers = new List<IBMS360Tower>();

    protected override void Start()
    {
        base.Start();
        activeBuffTowers.Add(this);
        AbilityManager.Instance.RegisterAbility(this);
        RoundManager.Instance.OnRoundEnd.AddListener(ResetBuffs);
        ApplyGlobalBuff();
    }

    private void OnDestroy()
    {
        activeBuffTowers.Remove(this);
        AbilityManager.Instance.UnregisterAbility(this);
        ApplyGlobalBuff();
    }

    public void Activate()
    {
        ActivateOverclock();
    }

    public string GetName()
    {
        return "System/360 Overclock";
    }

    public void ActivateOverclock()
    {
        if (isOverclockActive) return;
        StartCoroutine(OverclockRoutine());
    }

    private IEnumerator OverclockRoutine()
    {
        isOverclockActive = true;
        float originalFireRate = currentAttackSpeed;
        currentAttackSpeed /= overclockMultiplier;
        Debug.Log("IBM System/360 Overclock Activated!");

        yield return new WaitForSeconds(overclockDuration);

        currentAttackSpeed = originalFireRate;
        isOverclockActive = false;
        Debug.Log("IBM System/360 Overclock Ended.");
    }

    private void ResetBuffs()
    {
        currentAttackSpeed = attackSpeed;
        List<BaseTower> allTowers = new List<BaseTower>(FindObjectsOfType<BaseTower>());
        foreach (BaseTower tower in allTowers)
        {
            tower.Reset();           
        }
        Debug.Log("IBM System/360 Buffs Reset at Round End.");
    }

    private void ApplyGlobalBuff()
    {
        List<BaseTower> allTowers = new List<BaseTower>(FindObjectsOfType<BaseTower>());
        foreach (BaseTower tower in allTowers)
        {
            tower.SetAttackSpeed(tower.GetAttackSpeed() * (tower.GetAttackSpeed() + globalAttackBoost));
            tower.damage = Mathf.RoundToInt(tower.damage * (tower.damage + globalAttackBoost));
        }
    }

    public override void Shoot()
    {
        if (target == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        IBMS360Bullet bulletScript = bullet.GetComponent<IBMS360Bullet>();
        bulletScript.Initialize(target, damage, isOverclockActive);
        if (isOverclockActive)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.transform.position, explosionRadius);
            foreach (Collider2D hit in hitEnemies)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage / 2);                    
                }
            }
        }
    }
}
