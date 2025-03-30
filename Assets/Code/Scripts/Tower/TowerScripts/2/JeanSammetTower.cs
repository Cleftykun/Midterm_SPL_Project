using UnityEngine;
using System.Collections;

public class JeanSammetTower : BaseTower
{
    private int roundsPassed = 0;

    new void Start()
    {
        base.Start();
        RoundManager.Instance.OnRoundStart.AddListener(OnRoundStart);
    }

    protected void OnDestroy()
    {
        RoundManager.Instance.OnRoundStart.RemoveListener(OnRoundStart);
    }

    private void OnRoundStart()
    {
        roundsPassed++;
        baseDamage += 20; // Increases damage each round
    }
}
