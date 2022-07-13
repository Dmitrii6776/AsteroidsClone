using System;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeToDespawn;
    private Vector3 _direction;
    private PoolingManager _poolingManager;


    
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    public void DefineManager(PoolingManager poolingManager)
    {
        _poolingManager = poolingManager;
    }
    private void Start()
    {
        StartCoroutine(nameof(DespawnInTime));
    }

    private void Update()
    {
        transform.position += _direction * bulletSpeed * Time.deltaTime;

    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        
            Despawn();
        
        
    }

    private IEnumerator DespawnInTime()
    {
        yield return new WaitForSeconds(timeToDespawn);
        Despawn();
    }

    private void Despawn()
    {
        _poolingManager.Despawn(gameObject.transform);
    }
    
}
