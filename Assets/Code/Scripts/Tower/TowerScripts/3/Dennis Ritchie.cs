using UnityEngine;

public class DennisRitchieTower : BaseTower
{
    protected override void Start()
    {
        base.Start();
        // Apply enhancements for C-based towers (Hardware & Systems)
        EnhanceHardwareSystemsTowers();
    }

    // Enhance all C-based towers
    private void EnhanceHardwareSystemsTowers()
    {
        // Assuming we can access all towers or a specific subset of towers
        if (towerClassification == Classifaction.HardwareSystems)
        {
            baseAttackSpeed = 2f; // Set attack speed to 2
            damage = Mathf.RoundToInt(ApplyMemoryLeakDamage(damage)); 
                                                                     
        }
    }

    // Apply memory leak damage enhancement
    private float ApplyMemoryLeakDamage(float baseDamage)
    {
        // Example: Increase damage by a factor of 1.5x to simulate memory leak damage
        return baseDamage * 1.5f;
    }

    public override void Shoot()
    {
        if (target == null) return;
        // Shoot bullets with the enhanced memory leak damage
        base.Shoot();
    }
}
