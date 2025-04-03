using UnityEngine;
using System.Collections.Generic;

public class JohnMcCarthyTower : BaseTower
{
    [Header("John McCarthy Tower Settings")]
    public GameObject aiSentryPrefab; // Reference to the AI Sentry prefab
    public int maxSentries = 3; // Maximum number of active sentries
    private List<AISentry> activeSentries = new List<AISentry>(); // List to track active sentries

    // Update method for continuous spawning
    protected override void Update()
    {
        if (isDisabled) return;

        SpawnAISentry(); // Call the method to spawn sentries
    }

    private void SpawnAISentry()
    {
        // Check how many sentries are currently active
        if (activeSentries.Count < maxSentries)
        {
            // Instantiate a new AI Sentry and add it to the list of active sentries
            GameObject aiSentryObject = Instantiate(aiSentryPrefab, transform.position, Quaternion.identity);
            AISentry aiSentryScript = aiSentryObject.GetComponent<AISentry>();

            if (aiSentryScript != null)
            {
                activeSentries.Add(aiSentryScript); // Add to the list of active sentries
                Debug.Log("AI Sentry spawned!");

            }
            else
            {
                Debug.LogError("AISentry script not found on the prefab.");
            }
        }
        else
        {
            Debug.Log("Maximum number of sentries reached.");
        }
    }
}
