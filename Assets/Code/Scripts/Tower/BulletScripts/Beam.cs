using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{
    private float beamDuration = 3f; // How long the beam stays
    private float maxLength = 3f; // How far the beam stretches
    private int damage;

    public void Initialize(int beamDamage)
    {
        damage = beamDamage;
        StartCoroutine(StretchBeam());
        Destroy(gameObject, beamDuration); // Destroy after duration
    }

    private IEnumerator StretchBeam()
    {
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(maxLength, startScale.y, startScale.z);

        while (elapsedTime < beamDuration)
        {
            float t = elapsedTime / beamDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // Ensure it reaches full length
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
}
