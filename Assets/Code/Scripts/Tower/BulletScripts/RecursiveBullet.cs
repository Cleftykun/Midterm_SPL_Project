using UnityEngine;

public class RecursiveBullet : BaseBullet
{
    public int maxSplits;
    public float splitAngle;
    private int currentSplits = 0;
   

    public void Initialize(Transform target, int damage, int maxSplits, float splitAngle)
    {
        this.target = target;
        this.damage = damage;
        this.maxSplits = maxSplits;
        this.splitAngle = splitAngle;
        if (target != null)
        {
                Vector2 direction = (target.position - transform.position).normalized;
                rb.linearVelocity = direction * bulletSpeed;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other){
        if (other.transform == target)
        {
            ApplyDamage();
        }        
        if (currentSplits < maxSplits)
        {
            SplitProjectile();
            Destroy(gameObject);
        }
    }

    private void SplitProjectile()
    {
        currentSplits++;
        float angleOffset = splitAngle / 2;

        Quaternion leftRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - angleOffset);
        Quaternion rightRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angleOffset);

        CreateSplitProjectile(leftRotation);
        CreateSplitProjectile(rightRotation);
    }

    private void CreateSplitProjectile(Quaternion rotation)
    {
        GameObject splitBullet = Instantiate(gameObject, transform.position, rotation);
        RecursiveBullet splitBulletScript = splitBullet.GetComponent<RecursiveBullet>();
        splitBulletScript.Initialize(target, damage, maxSplits, splitAngle);
        splitBulletScript.currentSplits = this.currentSplits;

        // Ensure the split bullets are destroyed after the lifetime
        Destroy(splitBullet, bulletLifetime);
    }
}
