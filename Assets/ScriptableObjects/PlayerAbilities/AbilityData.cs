using UnityEngine;
using Spine;
using Spine.Unity;
using System;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string abilityName;

    [SpineAnimation] 
    public AnimationReferenceAsset animationAsset; // Stores the animation directly

    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    public bool loopAnimation = false;

    [Header("Cooldown Settings")]
    public float cooldownDuration = 1.5f;
    private float lastUsedTime = -Mathf.Infinity;

    public event Action<string, GameObject> OnAbilityEvent; // Event handler for animation events

    // Check if ability is off cooldown
    public bool IsOffCooldown()
    {
        return Time.time >= lastUsedTime + cooldownDuration;
    }

    // Call this when using the ability
    public void UseAbility(GameObject user)
    {
        lastUsedTime = Time.time;
        ApplyEffect(user);
    }

    // Apply effects of ability (placeholder)
    public void ApplyEffect(GameObject target)
    {
        Debug.Log($"{abilityName} effect applied to {target.name}");
    }

    // Triggered when an animation event occurs
    public void TriggerEvent(string eventName, GameObject user)
    {
        OnAbilityEvent?.Invoke(eventName, user);
    }
}
