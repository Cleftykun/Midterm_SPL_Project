using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ObjectKey
{
    public GameObject targetObject;
    public KeyCode toggleKey;
    public Button toggleButton; // Optional UI button
}

public class Toggle : MonoBehaviour
{
    public List<ObjectKey> objectKey = new List<ObjectKey>();
    void Start()
    {
        // Ensure all screens are hidden but active at the start
        foreach (var pair in objectKey)
        {
            if (pair.targetObject != null)
                SetVisibility(pair.targetObject, false);

            // If a button is assigned, link it to ToggleScreen()
            if (pair.toggleButton != null)
            {
                ObjectKey capturedPair = pair; // Capture to avoid closure issues
                pair.toggleButton.onClick.AddListener(() => ToggleUI(capturedPair));
            }
        }
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
    void Update()
    {
        foreach (var pair in objectKey)
        {
            if (Input.GetKeyDown(pair.toggleKey) && pair.targetObject != null)
            {
                ToggleUI(pair);
            }
        }
    }
    private void ToggleUI(ObjectKey pair){
        SetVisibility(pair.targetObject,true);
        pair.targetObject.SetActive(!pair.targetObject.activeSelf);
    }
}

