using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDetails : MonoBehaviour
{
    public TowerInventory inventory;
    public Camera towerCamera;
    public RawImage cameraDisplayPanel;
    public Button upgradeButton;
    public Button destroyButton;
    public Button abilityBind;//?maybe?
    public TMP_Text towerName;
    public TMP_Text towerDescription;
    public TMP_Text atkOrig;
    public TMP_Text atkBuff;
    public TMP_Text spdOrig;
    public TMP_Text spdBuff;
    public TMP_Text rangeOrig;
    public TMP_Text rangeBuff;
    public TMP_Text towerCurrentLevel;
    public TMP_Text towerAvailableCopy;
    public Button CloseTowerUI;
    public GameObject TowerUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseTowerUI.onClick.AddListener(() => closeTowerUI());
        closeTowerUI();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void closeTowerUI()
    {
        TowerUI.SetActive(false);
    }

    private BaseTower tower;
    public void showTowerUI(BaseTower tower)
    {
        if (tower != null)
        {
            TowerUI.SetActive(true);
            this.tower = tower;
            UpdateTowerDetails();
            AdjustCamera();
            AssignButtons();
        }
    }
    private void AdjustCamera()
    {
        Vector3 towerPos = tower.transform.position; // Get Tower's position
        towerCamera.transform.position = new Vector3(towerPos.x, towerPos.y, towerCamera.transform.position.z);
        if (towerCamera == null || cameraDisplayPanel == null) return;

        RectTransform rt = cameraDisplayPanel.rectTransform;
        float uiAspect = rt.rect.width / rt.rect.height;
        towerCamera.aspect = uiAspect;
    }
    private void UpdateTowerDetails()
    {
        towerName.text = tower.GetTowerName();
        towerCurrentLevel.text = "Level: " + tower.GetUpgradeLevel();
        towerAvailableCopy.text = "Copies: " + inventory.GetTowerCount(tower.GetTowerName());
        towerDescription.text = tower.GetDescription();
        atkOrig.text = tower.baseDamage.ToString();
        atkBuff.text = "+(" + (tower.GetDamage()-tower.baseDamage).ToString() + ")";
        spdOrig.text = tower.baseAttackSpeed.ToString();
        spdBuff.text = "+(" + (tower.GetAttackSpeed()-tower.baseAttackSpeed).ToString() + ")";
        rangeOrig.text = tower.baseAttackRange.ToString();
        rangeBuff.text = "+(" + (tower.GetAttackRange()-tower.baseAttackRange).ToString() + ")";
        Debug.Log("Showing Details:" + tower.name + " " + tower.GetUpgradeLevel() + " " + inventory.GetTowerCount(tower.name));
    }
    private void AssignButtons()
    {
        upgradeButton.onClick.AddListener(() => UpgradeTower());
        destroyButton.onClick.AddListener(() => RemoveTower());
    }
    private void UpgradeTower()
    {
        if (tower != null)
        {
            tower.UpgradeTower();
        }
    }
    private void RemoveTower()
    {
        Debug.Log("Removing tower: " + tower.name);
        Destroy(tower.gameObject);
        closeTowerUI();
    }
    void OnEnable()
    {
        PhoneManager.PhoneOpened += closeTowerUI;
        PhoneManager.PhoneClosed += () => showTowerUI(tower);
        TowerInventory.OnInventoryChanged += UpdateTowerDetails;
    }

    void OnDisable()
    {
        PhoneManager.PhoneOpened -= closeTowerUI;
        PhoneManager.PhoneClosed -= () => showTowerUI(tower);
        TowerInventory.OnInventoryChanged -= UpdateTowerDetails;
    }

}
