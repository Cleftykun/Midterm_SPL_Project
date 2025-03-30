using UnityEngine;

public class JohnBackusTower : BaseSupport
{
    [Header("Support Effect")]
    [SerializeField] private float baseAttackSpeedBoost = 0.25f; // +25% Attack Speed
    [SerializeField] private float baseRecursionBonus = 0.2f; // -20% Cooldown Time
    [SerializeField] private float scalingEffect = 0.05f; // +5% per 3 waves (stacks up to 50%)
    
    private float totalAttackSpeedBoost;
    private float totalRecursionBonus;
    private int waveCount = 0;

    new void Start()
    {
        // Apply buff immediately when placed
        ApplySupportEffect();

        // Register for round events
        RoundManager.Instance.OnRoundStart.AddListener(OnRoundStart);
        RoundManager.Instance.OnRoundEnd.AddListener(OnRoundEnd);
    }

    new void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        RoundManager.Instance.OnRoundStart.RemoveListener(OnRoundStart);
        RoundManager.Instance.OnRoundEnd.RemoveListener(OnRoundEnd);
    }

    private void OnRoundStart()
    {
        waveCount++;
        ApplySupportEffect();
    }

    private void OnRoundEnd()
    {
        RemoveSupportEffect();
    }

    protected override void ApplySupportEffect()
    {
        CalculateScalingBonus();
        foreach (BaseTower tower in FindObjectsOfType<BaseTower>())
        {
            if (tower.GetClassifaction() == BaseTower.Classifaction.StructuredProcedural)
            {
                tower.SetAttackSpeed(tower.GetAttackSpeed() * (1 + totalAttackSpeedBoost));
                Debug.Log($"[John Backus] {tower.towerName} boosted! (+{totalAttackSpeedBoost * 100}% Attack Speed)");
            }
        }
    }

    protected override void RemoveSupportEffect()
    {
        foreach (BaseTower tower in FindObjectsOfType<BaseTower>())
        {
            if (tower.GetClassifaction() == BaseTower.Classifaction.StructuredProcedural)
            {
                tower.SetAttackSpeed(tower.GetAttackSpeed() / (1 + totalAttackSpeedBoost));
                Debug.Log($"[John Backus] {tower.towerName} lost its buff.");
            }
        }
    }

    private void CalculateScalingBonus()
    {
        int scalingFactor = waveCount / 3; // Every 3 waves, increase effect
        totalAttackSpeedBoost = baseAttackSpeedBoost + (scalingFactor * scalingEffect);
        totalRecursionBonus = baseRecursionBonus + (scalingFactor * scalingEffect);
        totalAttackSpeedBoost = Mathf.Min(totalAttackSpeedBoost, 0.5f); // Max +50%
        totalRecursionBonus = Mathf.Min(totalRecursionBonus, 0.5f); // Max -50% cooldown
    }
}
