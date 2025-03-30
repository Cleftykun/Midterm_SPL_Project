using System.Collections.Generic;
using UnityEngine;
public class CompiledBullet : BaseBullet
{
    private float compileTime;
    private float maxDamageMultiplier;
    private float travelTime;
    private int currentDamage;
    public void Initialize(Transform target, int baseDamage, float compileTime, float maxMultiplier)
    {
        this.target = target;
        this.damage = baseDamage;
        this.compileTime = compileTime;
        this.maxDamageMultiplier = maxMultiplier;
        travelTime = 0f;
    }

    new void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        travelTime += Time.deltaTime;
        float damageMultiplier = Mathf.Lerp(1f, maxDamageMultiplier, Mathf.Clamp01(travelTime / compileTime));
        currentDamage = Mathf.RoundToInt(damage * damageMultiplier);
        
        transform.position = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * Time.deltaTime);
    }

    protected override void ApplyDamage()
    {
        Enemy enemyHealth = target.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            Debug.Log("Damage: " + currentDamage);
            enemyHealth.TakeDamage(currentDamage);
        }
    }
}
