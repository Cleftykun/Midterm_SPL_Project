using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[System.Serializable]
public class ScreenKey
{
    public GameObject targetObject;
    public KeyCode toggleKey;
    public Button toggleButton; // Optional UI button
}

public class PhoneManager : MonoBehaviour
{
    public static event Action PhoneOpened;
    public static event Action PhoneClosed;
    public GameObject PhoneUI;
    public List<ScreenKey> ScreenKey = new List<ScreenKey>();
    public GameObject Gacha;
    public GameObject Messages;
    private GameObject activeScreen = null;
    public PlayerMovement playerMovement;

    void Start()
    {
        SetVisibility(Gacha,false);
        SetVisibility(Messages,false);
        SetVisibility(PhoneUI, false);
    }
    void OnEnable()
    {
        KeybindManager.OnGacha+= ()=>ToggleScreen(Gacha);
        KeybindManager.OnMessages+= ()=>ToggleScreen(Messages);
    }
    void OnDisable()
    {
        KeybindManager.OnGacha-= ()=>ToggleScreen(Gacha);
        KeybindManager.OnMessages-= ()=>ToggleScreen(Messages);
    }

    void ToggleScreen(GameObject screen)
    {
        if (activeScreen == screen && IsVisible(screen))
        {
            SetVisibility(PhoneUI, false);
            SetVisibility(activeScreen, false);
            activeScreen = null;
            playerMovement.PhoneUnEquip();
            PhoneClosed?.Invoke();
        }
        else
        {
            SetVisibility(PhoneUI, true);
            Activate(screen);

            PhoneOpened?.Invoke();
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
