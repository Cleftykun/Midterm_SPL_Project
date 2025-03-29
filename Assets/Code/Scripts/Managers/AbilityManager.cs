using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    private Dictionary<KeyCode, IAbility> abilityBindings = new Dictionary<KeyCode, IAbility>();
    private List<IAbility> registeredAbilities = new List<IAbility>();

    private KeyCode[] keyMappings = {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0
    };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        foreach (var key in abilityBindings.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                abilityBindings[key].Activate();
            }
        }
    }

    public void RegisterAbility(IAbility ability)
    {
        if (registeredAbilities.Contains(ability)) return;

        registeredAbilities.Add(ability);

        if (registeredAbilities.Count <= keyMappings.Length)
        {
            KeyCode assignedKey = keyMappings[registeredAbilities.Count - 1];
            abilityBindings[assignedKey] = ability;
            Debug.Log($"Ability {ability.GetName()} assigned to {assignedKey}");
        }
        else
        {
            Debug.LogWarning("Max ability slots reached!");
        }
    }

    public void UnregisterAbility(IAbility ability)
    {
        if (registeredAbilities.Contains(ability))
        {
            int index = registeredAbilities.IndexOf(ability);
            registeredAbilities.Remove(ability);

            if (index < keyMappings.Length)
            {
                abilityBindings.Remove(keyMappings[index]);
            }

            Debug.Log($"Ability {ability.GetName()} removed.");
            ReassignKeys();
        }
    }

    private void ReassignKeys()
    {
        abilityBindings.Clear();
        for (int i = 0; i < registeredAbilities.Count && i < keyMappings.Length; i++)
        {
            abilityBindings[keyMappings[i]] = registeredAbilities[i];
            Debug.Log($"Reassigned {registeredAbilities[i].GetName()} to {keyMappings[i]}");
        }
    }
}
