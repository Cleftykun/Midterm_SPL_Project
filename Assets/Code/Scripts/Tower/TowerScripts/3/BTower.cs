using System.Collections.Generic;
using UnityEngine;

public class BTower : BaseTower
{
    public float compileTime = 3f; // Time for max damage scaling
    public float maxDamageMultiplier = 2f; // Max damage increase
    
    protected override void Start()
    {
        base.Start();
    }

    public override void Shoot()
    {
        if (target == null) return;
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        CompiledBullet bulletScript = bullet.GetComponent<CompiledBullet>();
        bulletScript.Initialize(target, damage, compileTime, maxDamageMultiplier);
    }
}
