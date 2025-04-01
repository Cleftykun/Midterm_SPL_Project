using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmalltalkTower : BaseTower
{
    public GameObject miniTurretPrefab;
    public int maxSpawns = 3; // Maximum number of active MiniTurrets
    public float spawnRadius = 2f; // Distance around the tower
    public float spawnInterval = 5f; // Time between spawns
    private List<MiniTurret> activeSpawns = new List<MiniTurret>();

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnMiniTurrets());
    }

    private IEnumerator SpawnMiniTurrets()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (activeSpawns.Count < maxSpawns)
            {
                SpawnMiniTurret();
            }
        }
    }

    private void SpawnMiniTurret()
    {
        Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
        GameObject miniTurretObj = Instantiate(miniTurretPrefab, spawnPosition, Quaternion.identity);
        MiniTurret miniTurret = miniTurretObj.GetComponent<MiniTurret>();

        if (miniTurret != null)
        {
            miniTurret.Initialize(this);
            activeSpawns.Add(miniTurret);
        }
    }

    public void OnSpawnDestroyed(MiniTurret turret)
    {
        activeSpawns.Remove(turret);
    }

}
