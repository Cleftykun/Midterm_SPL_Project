using UnityEngine;

public abstract class BaseSupport : MonoBehaviour
{
    [Header("Tower Data")]
    public string towerName;
    protected bool isDisabled = true; // Can be toggled on/off if needed

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
    public void DisableTower()
    {
        isDisabled = true;
    }

    public void EnableTower()
    {
        isDisabled = false;
    }
}
