using System;
using System.Collections;
using System.Collections.Generic;
using AsteroidsClone;
using PathologicalGames;
using UnityEngine;
using Random = UnityEngine.Random;

public class UFOComponent : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private BulletComponent bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;

    private PlayerComponent _player;
    private UFOManager _ufoManager;
    private Vector3 _direction;
    private ScreenTeleport _screenTeleport;
    private PoolingManager _poolingManager;



    public void DefineManagers(UFOManager manager, PoolingManager poolingManager)
    {
        _ufoManager = manager;
        _poolingManager = poolingManager;
    }
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
        Shoot();
    }

    public void DefinePlayer(PlayerComponent playerComponent)
    {
        _player = playerComponent;
    }

    private void Start()
    {
        _screenTeleport = new ScreenTeleport(gameObject);
    }

    private void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
        ScreenEndTeleport();
    }

    private void Shoot()
    {
        var bullet = _poolingManager
            .Spawn(bulletPrefab.gameObject, bulletSpawnPosition.position, Quaternion.identity)
            .GetComponent<BulletComponent>();
        bullet.DefineManager(_poolingManager);
        bullet.SetDirection((_player.transform.position - transform.position).normalized);
        _ufoManager.PlayGunShotSound();
        StartCoroutine(nameof(ShootDelay));
    }

    private int GetRandomTimeInSec()
    {
        return Random.Range(2, 5);
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(GetRandomTimeInSec());
            Shoot();
    }

    

    private void ScreenEndTeleport()
    {
        _screenTeleport.CheckScreenBorders();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("UfoBullet")) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _ufoManager.Despawn(gameObject, true);
        }
        _ufoManager.Despawn(gameObject, false);

    }
}
