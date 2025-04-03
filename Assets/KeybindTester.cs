using UnityEngine;

public class KeybindTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        KeybindManager.OnPauseAndPlay += TestPauseAndPlay;
        KeybindManager.OnSpeed += TestSpeed;
        KeybindManager.OnInventory += TestInventory;
        KeybindManager.OnMessages += TestMessages;
        KeybindManager.OnGacha += TestGacha;
        KeybindManager.OnMenu += TestMenu;
        
        KeybindManager.OnAbility1 += TestAbility1;
        KeybindManager.OnAbility2 += TestAbility2;
        
        KeybindManager.OnTowerAbility1 += TestTowerAbility1;
        KeybindManager.OnTowerAbility2 += TestTowerAbility2;
        KeybindManager.OnTowerAbility3 += TestTowerAbility3;
        KeybindManager.OnTowerAbility4 += TestTowerAbility4;
        KeybindManager.OnTowerAbility5 += TestTowerAbility5;
        KeybindManager.OnTowerAbility6 += TestTowerAbility6;
        KeybindManager.OnTowerAbility7 += TestTowerAbility7;
        KeybindManager.OnTowerAbility8 += TestTowerAbility8;
        KeybindManager.OnTowerAbility9 += TestTowerAbility9;
        KeybindManager.OnTowerAbility0 += TestTowerAbility0;
    }

    void OnDisable()
    {
        KeybindManager.OnPauseAndPlay -= TestPauseAndPlay;
        KeybindManager.OnSpeed -= TestSpeed;
        KeybindManager.OnInventory -= TestInventory;
        KeybindManager.OnMessages -= TestMessages;
        KeybindManager.OnGacha -= TestGacha;
        KeybindManager.OnMenu -= TestMenu;
        
        KeybindManager.OnAbility1 -= TestAbility1;
        KeybindManager.OnAbility2 -= TestAbility2;
        
        KeybindManager.OnTowerAbility1 -= TestTowerAbility1;
        KeybindManager.OnTowerAbility2 -= TestTowerAbility2;
        KeybindManager.OnTowerAbility3 -= TestTowerAbility3;
        KeybindManager.OnTowerAbility4 -= TestTowerAbility4;
        KeybindManager.OnTowerAbility5 -= TestTowerAbility5;
        KeybindManager.OnTowerAbility6 -= TestTowerAbility6;
        KeybindManager.OnTowerAbility7 -= TestTowerAbility7;
        KeybindManager.OnTowerAbility8 -= TestTowerAbility8;
        KeybindManager.OnTowerAbility9 -= TestTowerAbility9;
        KeybindManager.OnTowerAbility0 -= TestTowerAbility0;
    }

    void TestPauseAndPlay() => Debug.Log("Pause/Play key pressed!");
    void TestSpeed() => Debug.Log("Speed key pressed!");
    void TestInventory() => Debug.Log("Inventory key pressed!");
    void TestMessages() => Debug.Log("Messages key pressed!");
    void TestGacha() => Debug.Log("Gacha key pressed!");
    void TestMenu() => Debug.Log("Menu key pressed!");
    
    void TestAbility1() => Debug.Log("Ability 1 key pressed!");
    void TestAbility2() => Debug.Log("Ability 2 key pressed!");

    void TestTowerAbility1() => Debug.Log("Tower Ability 1 key pressed!");
    void TestTowerAbility2() => Debug.Log("Tower Ability 2 key pressed!");
    void TestTowerAbility3() => Debug.Log("Tower Ability 3 key pressed!");
    void TestTowerAbility4() => Debug.Log("Tower Ability 4 key pressed!");
    void TestTowerAbility5() => Debug.Log("Tower Ability 5 key pressed!");
    void TestTowerAbility6() => Debug.Log("Tower Ability 6 key pressed!");
    void TestTowerAbility7() => Debug.Log("Tower Ability 7 key pressed!");
    void TestTowerAbility8() => Debug.Log("Tower Ability 8 key pressed!");
    void TestTowerAbility9() => Debug.Log("Tower Ability 9 key pressed!");
    void TestTowerAbility0() => Debug.Log("Tower Ability 0 key pressed!");
}

