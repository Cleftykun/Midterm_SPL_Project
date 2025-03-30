using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmalltalkSpawn : BaseTower
{
    private float lifetime = 10f;
    private SmalltalkTower parentTower;
    
    public void Initialize(SmalltalkTower parent)
    {
        parentTower = parent;
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifetime);
        parentTower.OnSpawnDestroyed();
        Destroy(gameObject);
    }
}