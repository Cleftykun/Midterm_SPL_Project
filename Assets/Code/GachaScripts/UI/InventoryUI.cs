using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public RectTransform inventoryPanel;
    public GameObject inventoryItemPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateInventoryUI(List<TowerInventory.TowerEntry> inventory)
    {
        // Check if inventoryPanel and inventoryItemPrefab are assigned
        if (inventoryPanel == null || inventoryItemPrefab == null)
        {
            UnityEngine.Debug.LogError("Inventory Panel or Inventory Item Prefab is not assigned.");
            return;
        }

        // Clear existing items
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // Create new items based on the inventory
        foreach (var entry in inventory)
        {
            if (entry.towerPrefab == null)
            {
                UnityEngine.Debug.LogError("Tower Prefab is null for tower entry with ID: " + entry.towerID);
                continue; // Skip this entry if towerPrefab is null
            }

            GameObject item = Instantiate(inventoryItemPrefab, inventoryPanel);

            // Check for the SpriteRenderer in the hierarchy that uses the 'body_black_01' sprite
            SpriteRenderer baseSpriteRenderer = FindSpriteRenderer(entry.towerPrefab, "body_black_01");
            if (baseSpriteRenderer == null)
            {
                UnityEngine.Debug.LogError("SpriteRenderer with sprite 'body_black_01' not found for tower prefab: " + entry.towerPrefab.name);
            }

            TextMeshProUGUI textComponent = item.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = $"x{entry.count}";
            }

            // Find the UI image components where you want to assign these sprites
            UnityEngine.UI.Image baseIcon = item.transform.Find("BaseIcon")?.GetComponent<UnityEngine.UI.Image>();
            UnityEngine.UI.Image gunIcon = item.transform.Find("GunIcon")?.GetComponent<UnityEngine.UI.Image>();

            // Assign the base sprite to the UI image (handle base)
            if (baseIcon != null && baseSpriteRenderer != null)
            {
                baseIcon.sprite = baseSpriteRenderer.sprite;
                baseIcon.color = baseSpriteRenderer.color;
            }

            SpriteRenderer gunSpriteRenderer = FindSpriteRenderer(entry.towerPrefab, "basic_black"); // Update if gun has different sprite
            if (gunIcon != null && gunSpriteRenderer != null)
            {
                gunIcon.sprite = gunSpriteRenderer.sprite;
                gunIcon.color = gunSpriteRenderer.color;
            }
            else if (gunIcon != null)
            {
                gunIcon.enabled = false; // Hide gun icon if no gun exists
            }

            // Assign functionality to the button
            Button button = item.GetComponent<Button>();
            if (button != null)
            {
                int towerID = entry.towerID;
                button.onClick.AddListener(() => BuildManager.Instance.SetSelectedTower(towerID));
            }
        }
    }

    // Helper method to find SpriteRenderer by sprite name
    private SpriteRenderer FindSpriteRenderer(GameObject towerPrefab, string spriteName)
    {
        // Recursively search all children for SpriteRenderer with the desired sprite name
        SpriteRenderer[] spriteRenderers = towerPrefab.GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in spriteRenderers)
        {
            if (renderer.sprite != null && renderer.sprite.name == spriteName)
            {
                return renderer;
            }
        }
        return null; // Return null if no SpriteRenderer with the matching sprite name is found
    }



}
