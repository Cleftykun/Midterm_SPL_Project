using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrologTower : BaseTower
{
    [Header("Logic Gate Settings")]
    public GameObject logicGatePrefab;
    public float gateCooldown = 5f;
    public float gateLifetime = 5f;
    public float weakenEffect = 0.2f; // Reduces enemy damage or speed
    public int maxGates = 10;

    private float gateTimer = 0f;
    private Queue<GameObject> activeGates = new Queue<GameObject>();

    protected override void Update()
    {
        if (isDisabled) return;

        gateTimer += Time.deltaTime;
        if (gateTimer >= gateCooldown)
        {
            PlaceLogicGate();
            gateTimer = 0f;
        }
    }

    private void PlaceLogicGate()
    {
        Transform spawnPoint = GetNearestPathPosition();
        if (spawnPoint == null) return;

        GameObject gate = Instantiate(logicGatePrefab, spawnPoint.position, Quaternion.identity);
        LogicGate logicGateScript = gate.GetComponent<LogicGate>();

        if (logicGateScript != null)
        {
            logicGateScript.Initialize(weakenEffect, gateLifetime);
        }

        activeGates.Enqueue(gate);
        if (activeGates.Count > maxGates)
        {
            GameObject oldestGate = activeGates.Dequeue();
            Destroy(oldestGate);
        }
    }

    private Transform GetNearestPathPosition()
    {
        if (LevelManager.main.paths.Count == 0) return null;

        Transform nearestWaypoint = null;
        float minDistance = float.MaxValue;

        foreach (var path in LevelManager.main.paths)
        {
            foreach (var waypoint in path.waypoints)
            {
                float distance = Vector2.Distance(transform.position, waypoint.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestWaypoint = waypoint;
                }
            }
        }

        return nearestWaypoint;
    }
}
