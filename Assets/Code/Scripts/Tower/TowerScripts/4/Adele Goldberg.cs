using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AdeleGoldbergTower : BaseTower
{
    private float buffRange = 5f; // AOE buff radius
    private float attackSpeedBoost = 0.3f; // 30% attack speed increase
    private float damageBoost = 0.2f; // 20% extra damage

    protected override void Start()
    {
        base.Start();
        towerClassification = Classifaction.ObjectOrientedModular;
        StartCoroutine(ApplyBuffs());
    }

    private IEnumerator ApplyBuffs()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            BuffNearbyTowers();
        }
    }

    private void BuffNearbyTowers()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, buffRange);
        foreach (var hit in hits)
        {
            BaseTower tower = hit.GetComponent<BaseTower>();
            if (tower != null && tower.GetClassifaction() == Classifaction.ObjectOrientedModular)
            {
                tower.SetAttackSpeed(tower.GetAttackSpeed() * (1 + attackSpeedBoost));
                tower.SetDamage(Mathf.RoundToInt(tower.GetDamage() * (1 + damageBoost)));
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        // Adele Goldberg Tower doesn't attack, only buffs allies
    }
}
