using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    private Dictionary<int, IAbility> abilityBindings = new Dictionary<int, IAbility>();
    private List<IAbility> registeredAbilities = new List<IAbility>();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        KeybindManager.OnTowerAbility0 += ActivateSlot0;
        KeybindManager.OnTowerAbility1 += ActivateSlot1;
        KeybindManager.OnTowerAbility2 += ActivateSlot2;
        KeybindManager.OnTowerAbility3 += ActivateSlot3;
        KeybindManager.OnTowerAbility4 += ActivateSlot4;
        KeybindManager.OnTowerAbility5 += ActivateSlot5;
        KeybindManager.OnTowerAbility6 += ActivateSlot6;
        KeybindManager.OnTowerAbility7 += ActivateSlot7;
        KeybindManager.OnTowerAbility8 += ActivateSlot8;
        KeybindManager.OnTowerAbility9 += ActivateSlot9;
    }

    void OnDisable()
    {
        KeybindManager.OnTowerAbility0 -= ActivateSlot0;
        KeybindManager.OnTowerAbility1 -= ActivateSlot1;
        KeybindManager.OnTowerAbility2 -= ActivateSlot2;
        KeybindManager.OnTowerAbility3 -= ActivateSlot3;
        KeybindManager.OnTowerAbility4 -= ActivateSlot4;
        KeybindManager.OnTowerAbility5 -= ActivateSlot5;
        KeybindManager.OnTowerAbility6 -= ActivateSlot6;
        KeybindManager.OnTowerAbility7 -= ActivateSlot7;
        KeybindManager.OnTowerAbility8 -= ActivateSlot8;
        KeybindManager.OnTowerAbility9 -= ActivateSlot9;
    }

    private void ActivateSlot(int slot)
    {
        if (abilityBindings.ContainsKey(slot))
        {
            abilityBindings[slot].Activate();
        }
    }


    private void ActivateSlot1() => ActivateSlot(0);
    private void ActivateSlot2() => ActivateSlot(1);
    private void ActivateSlot3() => ActivateSlot(2);
    private void ActivateSlot4() => ActivateSlot(3);
    private void ActivateSlot5() => ActivateSlot(4);
    private void ActivateSlot6() => ActivateSlot(5);
    private void ActivateSlot7() => ActivateSlot(6);
    private void ActivateSlot8() => ActivateSlot(7);
    private void ActivateSlot9() => ActivateSlot(8);
    private void ActivateSlot0() => ActivateSlot(9);
    public void RegisterAbility(IAbility ability)
    {
        if (registeredAbilities.Contains(ability)) return;
        int slot = registeredAbilities.Count;
        if (slot >= 10)
        {
            Debug.LogWarning("Max ability slots reached!");
            return;
        }

        registeredAbilities.Add(ability);
        abilityBindings[slot] = ability;

        Debug.Log($"Ability {ability.GetName()} assigned to slot {slot}");
    }

    public void UnregisterAbility(IAbility ability)
    {
        if (registeredAbilities.Contains(ability))
        {
            int index = registeredAbilities.IndexOf(ability);
            registeredAbilities.Remove(ability);
            abilityBindings.Remove(index);

            Debug.Log($"Ability {ability.GetName()} removed.");
            ReassignKeys();
        }
    }

    private void ReassignKeys()
    {
        abilityBindings.Clear();
        for (int i = 0; i < registeredAbilities.Count && i < 10; i++)
        {
            abilityBindings[i] = registeredAbilities[i];
            Debug.Log($"Reassigned {registeredAbilities[i].GetName()} to slot {i}");
        }
    }
}
