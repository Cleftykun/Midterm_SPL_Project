using UnityEngine;
using System.Collections.Generic;

public class ALGOLTower : BaseTower
{
    [Header("ALGOL Tower Settings")]
    public int maxSplits = 3;
    public float splitAngle = 30f; 
    
    protected override void Start()
    {
        base.Start();
    }
    
    public override void Shoot()
    {
        if (target == null) return;
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        RecursiveBullet bulletScript = bullet.GetComponent<RecursiveBullet>();
        bulletScript.Initialize(target, damage, maxSplits, splitAngle);
    }
}
