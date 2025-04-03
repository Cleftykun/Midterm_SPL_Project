using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIManager : MonoBehaviour
{
    public Slider waveProgress;
    public Slider killedProgress;
    public TMP_Text enemiesKilled;
    public TMP_Text totalEnemies;
    public TMP_Text currentWave;

    private int totalEnemyCount = 0;
    private int spawnedEnemiesCount = 0;
    private int killCount = 0;
    private int waveKillCount = 0;

    void Start()
    {
        EnemySpawner.onEnemyDestroy.AddListener(UpdateKillCount);
    }

    void OnEnable()
    {
        EnemySpawner.onWaveStart += UpdateWave;
        EnemySpawner.onEnemySpawn += UpdateWaveSpawnProgress;
    }

    void OnDisable()
    {
        EnemySpawner.onWaveStart -= UpdateWave;
        EnemySpawner.onEnemySpawn -= UpdateWaveSpawnProgress;
    }

    private void UpdateWave(int wave, int enemyCount)
    {
        currentWave.text = wave.ToString();
        totalEnemies.text = enemyCount.ToString();
        totalEnemyCount = enemyCount;
        resetProgress();
    }

    private void resetProgress()
    {
        waveProgress.maxValue = totalEnemyCount;
        waveProgress.value = 0;
        killedProgress.maxValue = totalEnemyCount;
        killedProgress.value = 0;
        spawnedEnemiesCount = 0;
        waveKillCount = 0;
        enemiesKilled.text = waveKillCount.ToString(); // Initialize enemiesKilled to 0
    }

    private void UpdateWaveSpawnProgress()
    {
        spawnedEnemiesCount++;
        waveProgress.value = spawnedEnemiesCount;
    }

    private void UpdateKillCount()
    {
        waveKillCount++;
        killCount++;
        enemiesKilled.text = waveKillCount.ToString();
        killedProgress.value = waveKillCount;
    }

    private void SetWaveFinish()
    {
    }
}