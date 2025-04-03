using UnityEngine;
using System.Collections;

public class FrancesAllenTower : BaseTower
{
    [Header("Frances Allen Tower Data")]
    [SerializeField] private float optimizationRadius = 10f;  // The radius in which optimization applies
    [SerializeField] private float cooldownReductionAmount = 0.1f; // 10% cooldown reduction
    

    private float burstCooldownTimer = 0f;

    private void Start()
    {
        base.Start();
        InvokeRepeating(nameof(OptimizeTowers), 1f, 1f);  // Optimize every 1 second
    }

    protected override void Update()
    {
        base.Update();

        // Handle burst mode cooldown
        if (burstCooldownTimer > 0f)
        {
            burstCooldownTimer -= Time.deltaTime;
        }
    }

    private void OptimizeTowers()
    {
        // Find all nearby towers (compiler-based)
        Collider2D[] nearbyTowers = Physics2D.OverlapCircleAll(transform.position, optimizationRadius);

        foreach (Collider2D col in nearbyTowers)
        {
            BaseTower nearbyTower = col.GetComponent<BaseTower>();
            if (nearbyTower != null && IsCompilerBasedTower(nearbyTower))
            {
                // Reduce the attack speed of nearby compiler-based towers
                nearbyTower.SetAttackSpeed(nearbyTower.GetAttackSpeed() * (1 - cooldownReductionAmount));
            }
        }
    }

    // Checks if the tower is compiler-based
    private bool IsCompilerBasedTower(BaseTower tower)
    {
        return tower.GetClassifaction() == Classifaction.AIOptimization ||
               tower.GetClassifaction() == Classifaction.FunctionalLogicBased ||
               tower.GetClassifaction() == Classifaction.StructuredProcedural ||
               tower.GetClassifaction() == Classifaction.ObjectOrientedModular;
    }


    // Apply resource efficiency (missed shots restore cooldown energy)
   

    // Visualize the optimization radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, optimizationRadius);
    }
}
