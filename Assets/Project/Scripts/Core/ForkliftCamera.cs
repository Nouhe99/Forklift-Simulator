using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Acreos.ForkliftSim.Vehicle
{
    public class ForkliftCamera : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _targetOffset = new Vector3(0, 1.5f, 0);

        [Header("Settings")]
        [SerializeField] private float _distance = 6f;
        [SerializeField] private float _minDistance = 2f;
        [SerializeField] private float _maxDistance = 12f;

        [Header("Sensitivity")]
        [SerializeField] private float _lookSpeed = 0.5f;
        [SerializeField] private float _zoomSpeed = 0.05f;
        [SerializeField] private float _smoothTime = 0.12f;

        private Vector3 _currentVelocity;
        private float _rotationY;
        private float _rotationX;
        private float _currentDistance;
        private bool _isActive = true;

        private void Start()
        {
            Vector3 angles = transform.eulerAngles;
            _rotationY = angles.y;
            _rotationX = 20f;
            _currentDistance = _distance;

        }

        public void SetInputActive(bool active)
        {
            _isActive = active;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            // Handle Input only if active and Mouse is detected
            if (_isActive && Mouse.current != null)
            {
                HandleInput();
            }

            // Clamping vertical angle
            _rotationX = Mathf.Clamp(_rotationX, 0f, 85f);

            // Calculate Position
            Quaternion rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
            Vector3 targetPosition = _target.position + _targetOffset;
            Vector3 desiredPosition = targetPosition - (rotation * Vector3.forward * _currentDistance);

            // Apply with Smoothing
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, _smoothTime);
            transform.LookAt(targetPosition);
        }

        private void HandleInput()
        {
            // Zoom (Scroll Wheel)
            float scrollValue = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scrollValue) > 0.1f)
            {
                _currentDistance -= scrollValue * _zoomSpeed * Time.deltaTime;
                _currentDistance = Mathf.Clamp(_currentDistance, _minDistance, _maxDistance);
            }

            // Rotation (Right Click Hold)
            if (Mouse.current.rightButton.isPressed)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                Vector2 delta = Mouse.current.delta.ReadValue();
                _rotationY += delta.x * _lookSpeed;
                _rotationX -= delta.y * _lookSpeed;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}