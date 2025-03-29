using UnityEngine;
using System.Collections.Generic;

public class PascalTower : BaseTower
{
    new void Shoot()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, target.position - firingPoint.position);
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, rotation);

        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(target, damage);
        }
    }
}
