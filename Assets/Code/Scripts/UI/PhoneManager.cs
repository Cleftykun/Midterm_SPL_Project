using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class ScreenKey
{
    public GameObject targetObject;
    public KeyCode toggleKey;
    public Button toggleButton; // Optional UI button
}

public class PhoneManager : MonoBehaviour
{
    public GameObject PhoneUI;
    public List<ScreenKey> ScreenKey = new List<ScreenKey>();
    private GameObject activeScreen = null;
    public PlayerMovement playerMovement;

    void Start()
    {
        // Ensure all screens are hidden but active at the start
        foreach (var pair in ScreenKey)
        {
            if (pair.targetObject != null)
                SetVisibility(pair.targetObject, false);

            // If a button is assigned, link it to ToggleScreen()
            if (pair.toggleButton != null)
            {
                ScreenKey capturedPair = pair; // Capture to avoid closure issues
                pair.toggleButton.onClick.AddListener(() => ToggleScreen(capturedPair));
            }
        }
        SetVisibility(PhoneUI, false);
    }

    void Update()
    {
        foreach (var pair in ScreenKey)
        {
            if (Input.GetKeyDown(pair.toggleKey) && pair.targetObject != null)
            {
                ToggleScreen(pair);
            }
        }
    }

    void ToggleScreen(ScreenKey pair)
    {
        if (activeScreen == pair.targetObject && IsVisible(pair.targetObject))
        {
            SetVisibility(PhoneUI, false);
            SetVisibility(activeScreen, false);
            activeScreen = null;
            playerMovement.PhoneUnEquip();
        }
        else
        {
            SetVisibility(PhoneUI, true);
            Activate(pair.targetObject);

            if (pair.toggleKey == KeyCode.F3)
                playerMovement.PhoneEquipSide();
            else
                playerMovement.PhoneEquip();
        }
    }

    void Activate(GameObject newActive)
    {
        // Hide all other screens
        foreach (var pair in ScreenKey)
        {
            if (pair.targetObject != null)
                SetVisibility(pair.targetObject, false);
        }

        // Show the new active screen
        SetVisibility(newActive, true);
        activeScreen = newActive;
    }

    void SetVisibility(GameObject obj, bool visible)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = visible ? 1 : 0;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    bool IsVisible(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        return canvasGroup != null && canvasGroup.alpha > 0;
    }
}
