using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    public static TowerUIManager Instance;

    [Header("UI Prefab Reference")]
    public GameObject towerOptionsPrefab; // Assign this in the Inspector

    private GameObject currentUIInstance;
    private Transform selectedTower;
    public TowerDetails towerDetails;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTowerUI(BaseTower tower)
    {
        if(tower == null) {
            UnityEngine.Debug.LogError("Tower is null");
            return;
        }
        towerDetails.showTowerUI(tower);
    }
}
