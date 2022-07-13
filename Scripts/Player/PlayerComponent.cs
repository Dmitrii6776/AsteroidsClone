

namespace AsteroidsClone
{
    
    using System.Collections;
    using UnityEngine;
    using Unity.Mathematics;

    public class PlayerComponent : MonoBehaviour
    {
        [Header("Player Parameters")]
        [SerializeField] private int lives;
        [SerializeField] private float minMoveSpeed;
        [SerializeField] private float maxMoveSpeed;
        [SerializeField] private float changeMoveSpeedStep;
        [SerializeField] private float rotationSpeed;
        [Header("Bullet Components")]
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private BulletComponent bulletPrefab;
        [Header("Managers")]
        [SerializeField] private UserInterface userInterface;
        [SerializeField] private PoolingManager poolingManager;
        [Header("Audio")]
        [SerializeField] private AudioSource gunShotSound;
        [SerializeField] private AudioSource accelerationSound;
        private float _currentMoveSpeed = 0;
        private Rigidbody2D _rb;
        private bool _canShoot = true;
        private Vector3 _currentDirection;
        private bool _isFlickering;
        private SpriteRenderer _renderer;
        private ScreenTeleport _screenTeleport;


       
        

        public void Boost()
        {
            _currentMoveSpeed = maxMoveSpeed;
            PlayAccelerationSound();
        }

        public void Slowing()
        {
            _currentMoveSpeed = minMoveSpeed;
        }
        public void ChangeSpeed()
        {
            if (_currentMoveSpeed == 0)
            {
                _currentMoveSpeed = minMoveSpeed;
                return;
            }

            if (_currentMoveSpeed >= minMoveSpeed && _currentMoveSpeed <= maxMoveSpeed)
            {
                _currentMoveSpeed += changeMoveSpeedStep * Time.deltaTime;
            }
        }

        public void SetDirection()
        {
            _currentDirection = transform.up;
            
        }

        public void Move()
        {

            var towardDirection = _currentDirection * _currentMoveSpeed;

            _rb.velocity = towardDirection;
        }

        public void RotateLeft()
        {

            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        }

        public void RotateRight()
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }

        public void MouseRotation(float angle)
        {
            transform.rotation = quaternion.Euler(0, 0, angle * 0.01f);
        }

        public void Shoot()
        {
            if (!_canShoot) return;
            var bullet = poolingManager
                .Spawn(bulletPrefab.gameObject, bulletSpawnPoint.position, Quaternion.identity)
                .GetComponent<BulletComponent>();
            bullet.DefineManager(poolingManager);
            bullet.SetDirection(transform.up);
            PlayGunShotSound();
            _canShoot = false;
            StartCoroutine(SwitchCanShoot());

        }
        private void PlayGunShotSound()
        {
            gunShotSound.Play();
        }

        private void PlayAccelerationSound()
        {
            if (!accelerationSound.isPlaying)
            {
                accelerationSound.Play(); 
            }
            
        }
        
        private void DecreaseLives()
        {
            lives -= 1;
            userInterface.SetLives(lives);
        }

        private IEnumerator ColorFlickerRoutine()
        {
            _isFlickering = true;
            StartCoroutine(nameof(FlickerOffRoutine));
            while (_isFlickering)
            {
                _renderer.color = Color.grey;
                yield return new WaitForSeconds(0.3f);
                _renderer.color = Color.white;
                yield return new WaitForSeconds(0.3f);
                
            }
        }

        private IEnumerator FlickerOffRoutine()
        {
            yield return new WaitForSeconds(3);
            _isFlickering = false;
        }
        

        private void Start()
        {
            _screenTeleport = new ScreenTeleport(gameObject);
            _rb = GetComponent<Rigidbody2D>();
            SetDirection();
            _currentMoveSpeed = minMoveSpeed;
            _renderer = GetComponent<SpriteRenderer>();
            StartCoroutine(nameof(ColorFlickerRoutine));
            userInterface.SetLives(lives);
            



        }

        private void Update()
        {
            ScreenEndTeleport();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_isFlickering)
            {
                if (lives > 1)
                {
                    DecreaseLives();
                    StartCoroutine(nameof(ColorFlickerRoutine));
                }
                else
                {
                    userInterface.GameOver();  
                }

                DecreaseLives();
                
            }
        }

        private IEnumerator SwitchCanShoot()
        {
            yield return new WaitForSeconds(0.2f);
            _canShoot = true;
        }

        private void ScreenEndTeleport()
        {
            _screenTeleport.CheckScreenBorders();
        }

    }
}