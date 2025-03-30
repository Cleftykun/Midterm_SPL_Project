using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Beam : BaseBullet
{
    private float maxLength = 6f; // How far the beam stretches
    private float damageInterval = 0.5f; // Time between damage ticks
    private HashSet<Enemy> damagedEnemies = new HashSet<Enemy>(); // Track damaged enemies
    public void Initialize(int beamDamage, Transform target)
    {
        this.target = target;
        base.Start();
        this.damage = beamDamage; // Use the inherited `damage` field from BaseBullet
        StartCoroutine(StretchBeam());
    }

    protected override void Update() 
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= bulletLifetime)
        {
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator StretchBeam()
    {
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(maxLength, startScale.y, startScale.z);

        while (elapsedTime < bulletLifetime)
        {
            float t = elapsedTime / bulletLifetime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Beam hit: {other.gameObject.name}");

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"[Jean Sammet Beam] Hit {enemy.name}, dealing {damage} damage!");
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                damagedEnemies.Add(enemy);
                StartCoroutine(ApplyContinuousDamage(enemy));
            }
        }
    }

    private IEnumerator ApplyContinuousDamage(Enemy enemy)
    {
        while (enemy != null && gameObject != null && damagedEnemies.Contains(enemy))
        {
            enemy.TakeDamage(damage);
            Debug.Log($"[Jean Sammet Beam] Continuously damaging {enemy.name} for {damage}!");
            yield return new WaitForSeconds(damageInterval);
        }
        damagedEnemies.Remove(enemy);
    }
}
