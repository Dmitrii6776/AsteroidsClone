
using UnityEngine;

namespace AsteroidsClone
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private UserInterface userInterface;
        [SerializeField] private PlayerComponent player;

        private bool _isMouseUsed;
        
        public void SwitchControl()
        {
            _isMouseUsed = !_isMouseUsed;
        }

        public bool GetCurrentControlState()
        {
            return _isMouseUsed;
        }

        private void Update()
        {
            if (_isMouseUsed)
            {
                ControlWithMouse();
                ControlWithoutMouse(); 
            }
            else
            {
                
                ControlWithoutMouse();
            }
            player.Move();
            LaunchPause();
        }

        private void ControlWithoutMouse()
        {
            if (Input.GetKey(KeyCode.W))
            {
                player.SetDirection();
                player.Boost();
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                player.Slowing();
            }

            if (Input.GetKey(KeyCode.A))
            {
                player.RotateLeft();
            }

            if (Input.GetKey(KeyCode.D))
            {
                player.RotateRight();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.Shoot();
            }
        }

        private void ControlWithMouse()
        {
            if (userInterface.isInMenu) return;
            var diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
            diff.Normalize();
            var angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            player.MouseRotation(angle);
            if (Input.GetMouseButtonDown(0))
            {
                player.Shoot();
            }

            if (Input.GetMouseButton(1))
            {
                player.SetDirection();
                player.Boost();
            }

            if (Input.GetMouseButtonUp(1))
            {
                player.Slowing();
            }
        }

        private void LaunchPause()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                userInterface.ActivatePauseMenu(); 
            }
            
        }

        

    }
}

