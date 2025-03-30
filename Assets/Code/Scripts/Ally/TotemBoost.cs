using System.Collections.Generic;
using UnityEngine;
public class TotemBoost : MonoBehaviour
{
    private float duration;
    private SimulaTower parentTower;
    private Dictionary<BaseTower, int> originalDamage = new Dictionary<BaseTower, int>();

    public void Initialize(float instanceDuration, SimulaTower tower)
    {
        duration = instanceDuration;
        parentTower = tower;
        ApplyBuff();
        Invoke("DestroyInstance", duration);
    }

    private void ApplyBuff()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, parentTower.auraRadius, LayerMask.GetMask("Tower"));

        foreach (var hitCollider in hitColliders)
        {
            BaseTower tower = hitCollider.GetComponent<BaseTower>();
            if (tower != null && tower != parentTower)
            {
                if (!originalDamage.ContainsKey(tower))
                {
                    originalDamage[tower] = tower.GetDamage(); // Store original damage
                }
                tower.SetDamage((int)(originalDamage[tower] * parentTower.buffAmount)); // Apply buff
            }
        }
    }

    private void DestroyInstance()
    {
        foreach (var entry in originalDamage)
        {
            entry.Key.SetDamage(entry.Value); // Reset damage
        }
        Destroy(gameObject);
    }
}
