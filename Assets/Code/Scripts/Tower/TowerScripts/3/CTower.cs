using System.Collections.Generic;
using UnityEngine;

public class CTower : BaseTower
{
    public float damageBoost = 1.2f; // Extra damage when near any tower
    public float auraRadius = 5f; // Radius in which damage boost applies

    private List<BaseTower> nearbyTowers = new List<BaseTower>();

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("CheckNearbyTowers", 0f, 1f);
    }

    private void CheckNearbyTowers()
    {
        nearbyTowers.Clear();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, auraRadius);
        
        foreach (var hitCollider in hitColliders)
        {
            BaseTower tower = hitCollider.GetComponent<BaseTower>();
            if (tower != null && tower != this)
            {
                nearbyTowers.Add(tower);
            }
        }
        
        // Update damage boost based on all nearby towers
        UpdateDamageBoost();
    }

    private void UpdateDamageBoost()
    {
        int towerCount = nearbyTowers.Count;
        damage = (int)(baseDamage * (1 + (towerCount * (damageBoost - 1))));
    }
}
