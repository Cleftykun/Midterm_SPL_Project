using UnityEngine;
using System.Collections.Generic;

public class LISPTower : BaseTower
{
    [Header("LISP Tower Settings")]
    public GameObject aiNodePrefab;
    public float nodeCooldown = 1f; 
    public float nodeSpawnRadius = 3f;
    public int maxNodes = 3;

    private float nodeTimer = 0f;
    private Queue<GameObject> activeNodes = new Queue<GameObject>();

    protected override void Update()
    {
        if (isDisabled) return;

        nodeTimer += Time.deltaTime;
        if (nodeTimer >= nodeCooldown)
        {
            SpawnAINode();
            nodeTimer = 0f;
        }
    }

    private void SpawnAINode()
    {
        Vector2 spawnOffset = Random.insideUnitCircle * nodeSpawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(spawnOffset.x, spawnOffset.y, 0);

        GameObject aiNode = Instantiate(aiNodePrefab, spawnPosition, Quaternion.identity);
        AINode aiScript = aiNode.GetComponent<AINode>();
        aiScript.Initialize(damage);

        activeNodes.Enqueue(aiNode);
        if (activeNodes.Count > maxNodes)
        {
            GameObject oldestNode = activeNodes.Dequeue();
            Destroy(oldestNode);
        }
    }
}
