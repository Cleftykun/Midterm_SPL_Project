using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DijkstraTower : BaseTower
{


    [Header("Firing Points")]
    [SerializeField] private Transform firingPoint1;
    [SerializeField] private Transform firingPoint2;

    [Header("Rotating Parts")]
    [SerializeField] private Transform turret1;
    [SerializeField] private Transform turret2;

 

    public override void Shoot()
    {
        if (target == null) return;

        ShootFrom(firingPoint1, turret1);
        ShootFrom(firingPoint2, turret2);
    }

    private void ShootFrom(Transform firingPoint, Transform turret)
    {
        if (firingPoint == null || turret == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(target, damage);
        }

       
    }

    protected override void RotateTowardsTarget()
    {
        if (target == null) return;

        RotateTurret(turret1);
        RotateTurret(turret2);
    }

    private void RotateTurret(Transform turret)
    {
        if (turret == null || target == null) return;

        Vector2 direction = (target.position - turret.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        turret.rotation = Quaternion.RotateTowards(
            turret.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

}
