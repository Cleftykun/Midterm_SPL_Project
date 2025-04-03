using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    public static TowerUIManager Instance;

    [Header("UI Prefab Reference")]
    public GameObject towerOptionsPrefab; // Assign this in the Inspector

    private GameObject currentUIInstance;
    private Transform selectedTower;
    // public TowerDetails towerDetails;
    public TowerDetails towerDetails;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTowerUI(BaseTower tower)
    {
        towerDetails.showTowerUI(tower);
    }
}
