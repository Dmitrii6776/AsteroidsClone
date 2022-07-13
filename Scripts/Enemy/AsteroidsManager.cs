
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AsteroidsManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PoolingManager poolingManager;
    [SerializeField] private UserInterface userInterface;
    [Header("Asteroid Variants")]
    [SerializeField] private List<AsteroidComponent> allPrefabAsteroids;
    [Header("Sounds")]
    [SerializeField] private AudioSource explosionSound;
    [Header("Destroy Reward")] 
    [SerializeField] private int smallAsteroidReward = 100;
    [SerializeField] private int middleAsteroidReward = 50;
    [SerializeField] private int bigAsteroidReward = 20;

    [Header("Spawn Delay")] 
    [SerializeField] private float minTimeSpawnDelay = 1;
    [SerializeField] private float maxTimeSpawnDelay = 5;
    
    private int _asteroidsCountForSwan = 2;
    private int _allAsteroidsInScene = 0;
    private static AsteroidsManager _instance;
    


    
    public void DestroyAsteroid(GameObject obj)
    {
        
        userInterface.SetScore(AsteroidSizeDeterminant(obj));
        if (!obj.activeSelf) return;
        poolingManager.Despawn(obj.transform);
        PlayExplosionSound();

    }
   

    public void ReduceAsteroidsCount()
    {
        _allAsteroidsInScene -= 1;
        if (_allAsteroidsInScene == 0)
        {
            MultiplySpawnAsteroids();
        }
    }
    public void AddAsteroidsCount()
    {
        _allAsteroidsInScene += 1;
    }

    private void PlayExplosionSound()
    {
        explosionSound.Play();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }


    private void Start()
    {
        MultiplySpawnAsteroids();
    }


    private Vector3 GetRandomDirection()
    {
        var camera = Camera.main;
 
        var randomScreenPosition = new Vector3(Random.Range(0f, camera.pixelWidth), Random.Range(0f, camera.pixelHeight), 0);
        var randomWorldPosition = camera.ScreenToWorldPoint(randomScreenPosition);
        return new Vector3(randomWorldPosition.x, randomWorldPosition.y, 0);

    }

    private void SpawnAsteroid(int index)
    {
        var spawnPoint = GetRandomSpawnPosition();
        var direction = GetRandomDirection();
        var asteroid = poolingManager
            .Spawn(allPrefabAsteroids[index].gameObject, spawnPoint, quaternion.identity).GetComponent<AsteroidComponent>();
        asteroid.DefineManagers(_instance, poolingManager);
        asteroid.SetDirection(direction);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var randomX = Random.Range(-8, 8);
        var randomY = Random.Range(-4, 4);
        var notRandomX = 8;
        var notRandomY = 4;
        Vector3[] randomPosList = new Vector3[]
        {
            new Vector3(notRandomX, randomY, 0), new Vector3(-notRandomX, randomY, 0),
            new Vector3(randomX, notRandomY, 0), new Vector3(randomX, -notRandomY, 0)
        };
        var randomIndex = Random.Range(0, randomPosList.Length);
        return randomPosList[randomIndex];
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, allPrefabAsteroids.Count);
    }

    private void MultiplySpawnAsteroids()
    {
        

        for (int i = 0; i < _asteroidsCountForSwan; i++)
        {
            var index = 0;
            if (_asteroidsCountForSwan > 3)
            {
                index = GetRandomIndex();
                StartCoroutine(nameof(SpawnDelay), index);
            }
            else
            {
                SpawnAsteroid(index);
            }

            
        }

        if (_asteroidsCountForSwan < 4)
        {
            _asteroidsCountForSwan += 1; 
        }

        
    }

    private IEnumerator SpawnDelay(int index)
    {
        yield return new WaitForSeconds(Random.Range(minTimeSpawnDelay, maxTimeSpawnDelay));
        SpawnAsteroid(index);
    }

    private int AsteroidSizeDeterminant(GameObject obj)
    {
        if (!obj.TryGetComponent(out AsteroidComponent asteroid)) return 0;
        return asteroid.asteroidSize switch
        {
            1 => bigAsteroidReward,
            2 => middleAsteroidReward,
            3 => smallAsteroidReward,
            _ => 0
        };
    }
    
}
