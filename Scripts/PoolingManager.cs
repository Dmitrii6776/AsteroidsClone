using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public Transform Spawn(GameObject obj, Vector3 position, Quaternion rotation)
    {
        return PoolManager.Pools["MainPool"].Spawn(obj, position, rotation);
    }

    public void Despawn(Transform obj)
    {
        PoolManager.Pools["MainPool"].Despawn(obj);
    }
}
