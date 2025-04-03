using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private WaveSerializable[] waves;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    public static event Action<int,int> onWaveStart;
    public static event Action onEnemySpawn;

    [Header("Reference")]
    [SerializeField] private GameObject winPanel;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool roundActive = false;
    private List<EnemySpawnData> spawnQueue = new List<EnemySpawnData>();
    private int activeEnemies = 0; // Track how many enemies are still alive

    public static EnemySpawner Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        onEnemyDestroy.AddListener(EnemyDestroyed); // Listen for enemy deaths
    }

    public void AddEnemy()
    {
        activeEnemies++;
    }
    void Update()
    {
        if (!isSpawning || !roundActive || spawnQueue.Count == 0) return;

        for (int i = spawnQueue.Count - 1; i >= 0; i--)
        {
            EnemySpawnData spawnData = spawnQueue[i];
            spawnData.timeSinceLastSpawn += Time.deltaTime;

            if (spawnData.timeSinceLastSpawn >= spawnData.spawnDelay && spawnData.remainingCount > 0)
            {
                // Get a random path
                int randomPathIndex = UnityEngine.Random.Range(0, LevelManager.main.paths.Count);
                List<Transform> selectedPath = LevelManager.main.paths[randomPathIndex].waypoints;

                // Spawn enemy
                GameObject enemy = Instantiate(spawnData.enemyPrefab, selectedPath[0].position, Quaternion.identity);
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.Initialize(randomPathIndex);
                }
                onEnemySpawn?.Invoke();
                spawnData.remainingCount--;
                spawnData.timeSinceLastSpawn = 0f;
            }

            if (spawnData.remainingCount <= 0)
            {
                spawnQueue.RemoveAt(i);
            }
        }

        if (spawnQueue.Count == 0)
        {
            StopSpawning();
        }
    }

    public void StartRound()
    {
        if (currentWaveIndex >= waves.Length) return;

        roundActive = true;
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(waves[currentWaveIndex].waveDelay);
        isSpawning = true;

        spawnQueue.Clear();
        int totalEnemiesInWave = 0;
        foreach (EnemySpawnInfo enemy in waves[currentWaveIndex].enemies)
        {
            totalEnemiesInWave += enemy.count;
            spawnQueue.Add(new EnemySpawnData(enemy.enemyPrefab, enemy.count, enemy.spawnDelay));
        }

        currentWaveIndex++;
        onWaveStart?.Invoke(currentWaveIndex, totalEnemiesInWave);

    }

    public void StopSpawning()
    {
        isSpawning = false;
        roundActive = false;
        spawnQueue.Clear();
    }

    private void EnemyDestroyed()
    {
        activeEnemies--; // Decrease count when an enemy dies

        if (activeEnemies <= 0 && currentWaveIndex >= waves.Length) // All enemies gone & last wave done
        {
            ChapterCompleted();
        }
    }

    private void ChapterCompleted()
    {
        Debug.Log("Chapter Completed!");
        PlayerPrefs.SetInt("CompletedChapter", SceneManager.GetActiveScene().buildIndex); // Save chapter
        PlayerPrefs.Save();

        if (winPanel != null)
        {
            winPanel.SetActive(true); // Show win panel
        }
    }
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int remainingCount;
    public float spawnDelay;
    public float timeSinceLastSpawn;

    public EnemySpawnData(GameObject prefab, int count, float delay)
    {
        enemyPrefab = prefab;
        remainingCount = count;
        spawnDelay = delay;
        timeSinceLastSpawn = 0f;
    }
}