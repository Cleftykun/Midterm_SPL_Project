using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseTower : MonoBehaviour
{
    public enum Classifaction
    {
        AIOptimization,
        AlgorithmicTheoreticalCS,
        FunctionalLogicBased,
        HardwareSystems,
        LowLevelComputing,
        ObjectOrientedModular,
        StructuredProcedural
    }
    public enum TargetingMode
    {
        First,
        Last,
        Strong,
        Weak,
        Near
    }

    [Header("Tower Data")]
    [SerializeField] public string towerName;
    [SerializeField] public float baseAttackRange;
    [SerializeField] public float baseAttackSpeed;
    [SerializeField] public int baseDamage;
    [SerializeField] public float rotationSpeed = 1000f;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Classifaction towerClassification;
    [Header("References")]
    [SerializeField] protected Transform turretRotationPoint;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected float currentAttackRange;
    [SerializeField] protected float currentAttackSpeed;
    [SerializeField] protected int damage;

    protected Transform target;
    protected float timeUntilFire;
    protected TargetingMode targetingMode = TargetingMode.First;
    
    private int upgradeLevel = 1;
    private const int maxUpgradeLevel = 10; //Up to 300%
    private const float upgradeMultiplier = 0.2f; 
    public bool isDisabled = false;
    protected virtual void Start()
    {   
        currentAttackRange = baseAttackRange;
        currentAttackSpeed = baseAttackSpeed;
        damage = baseDamage;
    }

    protected virtual void Update()
    {
        if (isDisabled) return;
        if (target == null || !CheckTargetIsInRange())
        {
            FindTarget();
        }

        if (target != null)
        {
            RotateTowardsTarget();
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / currentAttackSpeed)
            {
                Shoot();
                timeUntilFire = 0;
            }
        }

    }

    public virtual void Shoot()
    {
        if (target == null) return;
        GameObject bullet = Instantiate(bulletPrefab, target.position, firingPoint.rotation);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();
        bulletScript.Initialize(target, damage);
    }

    protected bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(target.position, transform.position) <= currentAttackRange;
    }

    protected virtual void RotateTowardsTarget()
    {
        if (target == null) return;

        float angle = (Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    protected virtual void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentAttackRange, enemyMask);
        if (hits.Length == 0)
        {
            target = null;
            return;
        }

        List<Enemy> enemies = new List<Enemy>();
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }

        if (enemies.Count == 0)
        {
            target = null;
            return;
        }

        switch (targetingMode)
        {
            case TargetingMode.First:
                target = enemies.OrderBy(e => e.GetDistanceToEnd()).FirstOrDefault()?.transform;
                break;
            case TargetingMode.Last:
                target = enemies.OrderByDescending(e => e.GetDistanceToEnd()).FirstOrDefault()?.transform;
                break;
            case TargetingMode.Strong:
                target = enemies.OrderByDescending(e => e.GetPlayerDamage()).FirstOrDefault()?.transform;
                break;
            case TargetingMode.Weak:
                target = enemies.OrderBy(e => e.GetPlayerDamage()).FirstOrDefault()?.transform;
                break;
            case TargetingMode.Near:
                target = enemies.OrderBy(e => Vector2.Distance(transform.position, e.transform.position))
                                .FirstOrDefault()?.transform;
                break;
        }
    }

    public float GetAttackRange(){return currentAttackRange;}
    public float GetAttackSpeed(){return currentAttackSpeed;}
    public Classifaction GetClassifaction(){return towerClassification;}
    public int GetDamage(){return damage;}
    public void SetAttackRange(float aR){currentAttackRange = aR;}
    public void SetAttackSpeed(float aS){currentAttackSpeed = aS;}
    public void SetDamage(int dmg){damage = dmg;}
    public void Reset()
    {
        if (upgradeLevel == 1) 
        {
            // Reset to base stats if it's level 1 (no upgrades applied)
            currentAttackRange = baseAttackRange;
            currentAttackSpeed = baseAttackSpeed;
            damage = baseDamage;
        }
        else
        {
            // Apply upgrade multiplier if it's beyond level 1
            currentAttackRange = baseAttackRange + (upgradeMultiplier * upgradeLevel);
            currentAttackSpeed = baseAttackSpeed + (upgradeMultiplier * upgradeLevel);
            damage = Mathf.RoundToInt(baseDamage + (upgradeMultiplier * upgradeLevel));
        }
        ResetTargetingMode();
    }
    private bool isSlowed = false;
    public virtual void ApplySlow(float factor, float duration)
    {        
        if (factor > currentSlowFactor && isSlowed) return;
        StartCoroutine(SlowEffect(factor, duration));
    }
    private float currentSlowFactor = 1.0f;
    private IEnumerator SlowEffect(float factor, float duration)
    {
        currentSlowFactor = factor;
        float speed = currentAttackSpeed;
        currentAttackSpeed *= factor;
        isSlowed = true;
        yield return new WaitForSeconds(duration);
        Reset();
        currentSlowFactor = 1.0f;
        isSlowed = false;
    }

    public void DisableTower()
    {
        isDisabled = true;
        target = null;
    }

    public void EnableTower()
    {
        isDisabled = false;
    }


    public bool UpgradeTower()
    {
        if (upgradeLevel >= maxUpgradeLevel)
        {
            Debug.Log("Tower is already at max upgrade level!");
            return false;
        }

        int towerCount = TowerInventory.Instance.GetTowerCount(towerName); // Get duplicates in inventory
        if (towerCount <= 0)
        {
            Debug.Log("No duplicate towers in inventory for upgrade!");
            return false;
        }

        TowerInventory.Instance.RemoveTowerFromUI(towerName);

        // Upgrade Stats
        upgradeLevel++;
        currentAttackRange = baseAttackRange + (upgradeMultiplier * upgradeLevel);
        currentAttackSpeed = baseAttackSpeed + (upgradeMultiplier * upgradeLevel);
        damage = Mathf.RoundToInt(baseDamage + upgradeMultiplier * upgradeLevel);

        return true;
    }

    public int GetUpgradeLevel() => upgradeLevel;
    public string GetTowerName() => towerName;
    public void RandomizeTargetingMode()
    {
        TargetingMode[] modes = (TargetingMode[])System.Enum.GetValues(typeof(TargetingMode));
        targetingMode = modes[Random.Range(0, modes.Length)];
    }
    public void ResetTargetingMode()
    {
        targetingMode = TargetingMode.First;
    }
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        TowerUIManager.Instance.ShowTowerUI(this);
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, currentAttackRange);
    }
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
