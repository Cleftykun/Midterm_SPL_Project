using UnityEngine;
using System;

public class KeybindManager : MonoBehaviour
{
    [Header("UI options")]
    public KeyCode pauseAndPlay = KeyCode.Space;
    public KeyCode speed = KeyCode.Tab;
    public KeyCode inventory = KeyCode.F1;
    public KeyCode messages = KeyCode.F2;
    public KeyCode gacha = KeyCode.F3;
    public KeyCode menu = KeyCode.Escape;

    [Header("Player Ability")]
    public KeyCode ability1 = KeyCode.E;
    public KeyCode ability2 = KeyCode.None; // Reserved?
    
    [Header("Tower Ability")]
    public KeyCode towerAbility1 = KeyCode.Alpha1;
    public KeyCode towerAbility2 = KeyCode.Alpha2;
    public KeyCode towerAbility3 = KeyCode.Alpha3;
    public KeyCode towerAbility4 = KeyCode.Alpha4;
    public KeyCode towerAbility5 = KeyCode.Alpha5;
    public KeyCode towerAbility6 = KeyCode.Alpha6;
    public KeyCode towerAbility7 = KeyCode.Alpha7;
    public KeyCode towerAbility8 = KeyCode.Alpha8;
    public KeyCode towerAbility9 = KeyCode.Alpha9;
    public KeyCode towerAbility0 = KeyCode.Alpha0;

    // Events
    public static event Action OnPauseAndPlay;
    public static event Action OnSpeed;
    public static event Action OnInventory;
    public static event Action OnMessages;
    public static event Action OnGacha;
    public static event Action OnMenu;
    
    public static event Action OnAbility1;
    public static event Action OnAbility2;
    
    public static event Action OnTowerAbility1;
    public static event Action OnTowerAbility2;
    public static event Action OnTowerAbility3;
    public static event Action OnTowerAbility4;
    public static event Action OnTowerAbility5;
    public static event Action OnTowerAbility6;
    public static event Action OnTowerAbility7;
    public static event Action OnTowerAbility8;
    public static event Action OnTowerAbility9;
    public static event Action OnTowerAbility0;
    void Update()
    {
        if (Input.GetKeyDown(pauseAndPlay)) OnPauseAndPlay?.Invoke();
        if (Input.GetKeyDown(speed)) OnSpeed?.Invoke();
        if (Input.GetKeyDown(inventory)) OnInventory?.Invoke();
        if (Input.GetKeyDown(messages)) OnMessages?.Invoke();
        if (Input.GetKeyDown(gacha)) OnGacha?.Invoke();
        if (Input.GetKeyDown(menu)) OnMenu?.Invoke();
        
        if (Input.GetKeyDown(ability1)) OnAbility1?.Invoke();
        if (Input.GetKeyDown(ability2)) OnAbility2?.Invoke();
        
        if (Input.GetKeyDown(towerAbility1)) OnTowerAbility1?.Invoke();
        if (Input.GetKeyDown(towerAbility2)) OnTowerAbility2?.Invoke();
        if (Input.GetKeyDown(towerAbility3)) OnTowerAbility3?.Invoke();
        if (Input.GetKeyDown(towerAbility4)) OnTowerAbility4?.Invoke();
        if (Input.GetKeyDown(towerAbility5)) OnTowerAbility5?.Invoke();
        if (Input.GetKeyDown(towerAbility6)) OnTowerAbility6?.Invoke();
        if (Input.GetKeyDown(towerAbility7)) OnTowerAbility7?.Invoke();
        if (Input.GetKeyDown(towerAbility8)) OnTowerAbility8?.Invoke();
        if (Input.GetKeyDown(towerAbility9)) OnTowerAbility9?.Invoke();
        if (Input.GetKeyDown(towerAbility0)) OnTowerAbility0?.Invoke();
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent multiple calls
        OnPauseAndPlay = null;
        OnSpeed = null;
        OnInventory = null;
        OnMessages = null;
        OnGacha = null;
        OnMenu = null;
        OnAbility1 = null;
        OnAbility2 = null;
        OnTowerAbility1 = null;
        OnTowerAbility2 = null;
        OnTowerAbility3 = null;
        OnTowerAbility4 = null;
        OnTowerAbility5 = null;
        OnTowerAbility6 = null;
        OnTowerAbility7 = null;
        OnTowerAbility8 = null;
        OnTowerAbility9 = null;
        OnTowerAbility0 = null;
}

}
