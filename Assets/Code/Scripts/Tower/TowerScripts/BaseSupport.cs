using UnityEngine;

public abstract class BaseSupport : BaseTower
{

    protected virtual void Start()
    {
        RegisterSupportEffect();
    }

    protected virtual void OnDestroy()
    {
        UnregisterSupportEffect();
    }

    protected abstract void ApplySupportEffect();
    protected abstract void RemoveSupportEffect();

    private void RegisterSupportEffect()
    {
        TowerManager.Instance.RegisterSupportTower(this);
        ApplySupportEffect();
    }

    private void UnregisterSupportEffect()
    {
        TowerManager.Instance.UnregisterSupportTower(this);
        RemoveSupportEffect();
    }

    public override void Shoot()
    {
        // Disable shooting functionality
    }

    protected override void RotateTowardsTarget()
    {
        // Disable rotation functionality
    }

    protected override void FindTarget()
    {
        // Disable target finding functionality
    }

    protected override void Update()
    {
        // Disable update functionality
    }

    public override void ApplySlow(float factor, float duration)
    {
        // Disable slow functionality
    }


    public override bool UpgradeTower()
    {
        // Disable upgrade functionality
        return false;
    }

   
}
