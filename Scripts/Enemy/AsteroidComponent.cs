
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidComponent : MonoBehaviour
{
    public int asteroidSize;
    [SerializeField] private List<AsteroidComponent> smallerAsteroids;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float flyawayAngle = 45f;
    
    private float _speed;
    private Vector3 _direction;
    private AsteroidsManager _asteroidsManager;
    private ScreenTeleport _screenTeleport;
    private PoolingManager _poolingManager;
    private Vector3 _previousPosition;

    public void DefineManagers(AsteroidsManager asteroidsManager, PoolingManager poolingManager)
    {
        _asteroidsManager = asteroidsManager;
        _poolingManager = poolingManager;
    }
    public void SetDirection(Vector3 direction)
    {
        _direction = (direction - transform.position).normalized;
        _asteroidsManager.AddAsteroidsCount();
        _previousPosition = gameObject.transform.position;

    }
    

    private void OnEnable()
    {
        _speed = GetRandomSpeed();


    }

    private void Start()
    {
        _screenTeleport = new ScreenTeleport(gameObject);

    }


    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
        ScreenEndTeleport();
    }

    private float GetRandomSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet")) {
            if (smallerAsteroids.Count > 0)
            {
                var newAsteroid1 = GetSmallerAsteroid(0);
                var tempSpeed = GetRandomSpeed();
                var asteroidAngle = gameObject.transform.eulerAngles;
                var currentPosition = gameObject.transform.position;
                var curentDirection = (currentPosition - _previousPosition).normalized;
                newAsteroid1.DefineManagers(_asteroidsManager, _poolingManager);
                newAsteroid1.SetDirection(Quaternion.Euler(asteroidAngle.x, asteroidAngle.y, asteroidAngle.z + flyawayAngle) * curentDirection * tempSpeed);
               

                var newAsteroid2 = GetSmallerAsteroid(1);
                newAsteroid2.DefineManagers(_asteroidsManager, _poolingManager);
                newAsteroid2.SetDirection(Quaternion.Euler(asteroidAngle.x, asteroidAngle.y, asteroidAngle.z -flyawayAngle) * curentDirection * tempSpeed);
               

            }
            _asteroidsManager.DestroyAsteroid(gameObject);
            _asteroidsManager.ReduceAsteroidsCount();
          
        } if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _asteroidsManager.DestroyAsteroid(gameObject);
            _asteroidsManager.ReduceAsteroidsCount();
        }
    }

    private AsteroidComponent GetSmallerAsteroid(int index)
    {
        return _poolingManager
            .Spawn(smallerAsteroids[index].gameObject, transform.position, Quaternion.identity)
            .GetComponent<AsteroidComponent>();
    }
    
    private void ScreenEndTeleport()
    {
        _screenTeleport.CheckScreenBorders();
        _previousPosition = gameObject.transform.position;
    }

}
