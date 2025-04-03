using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public GameObject inventory;
    public GameObject map;
    public GameObject menu;
    void Start()
    {
        ToggleUI(inventory);
    }
    
    void Update()
    {
    
    }
    void OnEnable()
    {
        KeybindManager.OnInventory+=()=>ToggleUI(inventory);
        KeybindManager.OnMenu+=()=>ToggleUI(menu);
        //KeybindManager.Onmap +=()=>ToggleUI(map); //soon
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
    private void ToggleUI(GameObject UI)
    {
        SetVisibility(UI, true);
        UI.SetActive(!UI.activeSelf);
    }
}

