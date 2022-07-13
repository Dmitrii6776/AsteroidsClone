
using System.Collections;
using AsteroidsClone;
using UnityEngine;
using Random = UnityEngine.Random;

public class UFOManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UFOComponent ufoPrefab;
    [SerializeField] private PlayerComponent player;
    [Header("Managers")]
    [SerializeField] private PoolingManager poolingManager;
    [SerializeField] private UserInterface userInterface;
    [Header("Audio")]
    [SerializeField] private AudioSource gunShotSound;
    [SerializeField] private AudioSource explosionSound;
    [Header("Destroy Reward")]
    [SerializeField] private int ufoDestroyReward = 200;

    [Header("Spawn Delay")] 
    [SerializeField] private int minTimeSpawnDelay = 20;

    [SerializeField] private int maxTimeSpawnDelay = 40;
    
    private Vector3 _spawnPosition;
    

    public void PlayGunShotSound()
    {
        gunShotSound.Play();
    }

    private void PlayExplosionSound()
    {
        explosionSound.Play();
    }
    
    public void Despawn(GameObject obj, bool killByplayer)
    {
        if (!obj.activeSelf) return;
        if (killByplayer)
        {
            userInterface.SetScore(ufoDestroyReward);
        }
        poolingManager.Despawn(obj.transform);
        PlayExplosionSound();
    }
    private void Start()
    {
        Invoke(nameof(SpawnUfo), 2);
    }

    private void SpawnUfo()
    {
        _spawnPosition = GetSpawnPosition();
        var ufo = poolingManager
            .Spawn(ufoPrefab.gameObject, _spawnPosition, Quaternion.identity).GetComponent<UFOComponent>();
        ufo.DefinePlayer(player);
        ufo.DefineManagers(this, poolingManager);
        ufo.SetDirection(GetDirection());
        StartCoroutine(nameof(SpawnDelay));

    }

    private Vector3 GetSpawnPosition()
    {
        var xPos = 6.83f;
        var randomYPos = Random.Range(-3, 3);
        Vector3[] randomPos = new[] {new Vector3(xPos, randomYPos, 0), new Vector3(-xPos, randomYPos, 0)};
        return randomPos[Random.Range(0, randomPos.Length)];
    }

    private Vector3 GetDirection()
    {
        return _spawnPosition.x < 0 ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);
    }

    private int GetRandomTimeInSec()
    {
        return Random.Range(minTimeSpawnDelay, maxTimeSpawnDelay);
    }

    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(GetRandomTimeInSec());
        SpawnUfo();
    }
    
}
