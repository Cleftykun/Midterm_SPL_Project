using System.Collections.Generic;
using UnityEngine;

public class SimulaTower : BaseTower
{
    public GameObject classInstancePrefab; 
    public float instanceDuration = 5f;
    public float instanceSpawnRate = 10f; 
    public float buffAmount = 1.15f; 
    public float auraRadius = 5f; 
    private List<BaseTower> nearbyTowers = new List<BaseTower>();

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("SpawnClassInstance", 0f, instanceSpawnRate);
    }

    private void SpawnClassInstance()
    {
        GameObject instance = Instantiate(classInstancePrefab, transform.position + new Vector3(1f, 0, 0), Quaternion.identity);
        TotemBoost classInstance = instance.AddComponent<TotemBoost>();
        classInstance.Initialize(instanceDuration, this);
        Destroy(instance, instanceDuration);
    }
}
