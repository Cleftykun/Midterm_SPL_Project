using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraceHopperTower : BaseTower
{
    [Header("Trap Settings")]
    public GameObject trapPrefab;
    public float trapCooldown = 5f;
    public float trapDuration = 3f;
    public float slowEffect = 0.9f; // 50% speed reduction
    public int maxTraps = 10;
    
    private float trapTimer = 0f;
    private Queue<GameObject> activeTraps = new Queue<GameObject>();

    protected override void Update()
    {
        if (isDisabled) return;
        
        trapTimer += Time.deltaTime;
        if (trapTimer >= trapCooldown)
        {
            PlaceTrap();
            trapTimer = 0f;
        }
    }

    private void PlaceTrap()
    {
        Transform spawnPoint = GetRandomPathPosition();
        if (spawnPoint == null) return;

        GameObject trap = Instantiate(trapPrefab, firingPoint.position, Quaternion.identity);
        DebuggingTrap trapScript = trap.GetComponent<DebuggingTrap>();
        trapScript.Initialize(trapDuration, spawnPoint);
        
        activeTraps.Enqueue(trap);
        if (activeTraps.Count > maxTraps)
        {
            GameObject oldestTrap = activeTraps.Dequeue();
            Destroy(oldestTrap);
        }
    }

    private Transform GetRandomPathPosition()
    {
        if (LevelManager.main.paths.Count == 0) return null;
        
        int randomPathIndex = Random.Range(0, LevelManager.main.paths.Count);
        List<Transform> path = LevelManager.main.paths[randomPathIndex].waypoints;
        
        if (path.Count == 0) return null;
        
        int randomPointIndex = Random.Range(0, path.Count);
        return path[randomPointIndex];
    }
}
