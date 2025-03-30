using UnityEngine;
using System.Collections;

public class JeanSammetTower : BaseTower
{
    [Header("Attack Stats")]
    [SerializeField] private GameObject beamPrefab; 
    private int roundsPassed = 0;

    new void Start()
    {
        RoundManager.Instance.OnRoundStart.AddListener(OnRoundStart);
        StartCoroutine(FireBeamLoop());
    }

    protected void OnDestroy()
    {
        RoundManager.Instance.OnRoundStart.RemoveListener(OnRoundStart);
    }

    private void OnRoundStart()
    {
        roundsPassed++;
        baseDamage += 2; // Increases damage each round
    }

    private IEnumerator FireBeamLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(baseAttackSpeed);
            FireBeam();
        }
    }

    private void FireBeam()
    {
        GameObject beam = Instantiate(beamPrefab, transform.position, transform.rotation);
        beam.GetComponent<Beam>().Initialize(baseDamage);
        Debug.Log($"[Jean Sammet] Fired Piercing Beam!");
    }

}
