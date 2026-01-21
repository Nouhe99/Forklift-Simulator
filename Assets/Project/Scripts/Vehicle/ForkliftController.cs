using UnityEngine;
using Acreos.ForkliftSim.Inputs;

namespace Acreos.ForkliftSim.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class ForkliftController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader _input;
        [SerializeField] private Transform _liftMechanism;

        [Header("Settings - Movement")]
        [SerializeField] private float _maxSpeed = 6f;
        [SerializeField] private float _acceleration = 2f;
        [SerializeField] private float _deceleration = 4f;

        [Header("Settings - Steering")]
        [SerializeField] private float _maxSteerAngle = 45f;
        [SerializeField] private float _steerSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 40f;

        [Header("Settings - Lift")]
        [SerializeField] private float _liftSpeed = 1f;
        [SerializeField] private float _minLiftHeight = 0f;
        [SerializeField] private float _maxLiftHeight = 3f;

        [Header("Visuals")]
        [SerializeField] private Transform _steeringWheel;
        [SerializeField] private Transform _rearAxle;

        private Rigidbody _rb;
        private float _currentSpeed;
        private float _currentSteerAngle;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.centerOfMass = new Vector3(0f, -0.5f, -1.0f);
        }

        private void OnEnable()
        {
            if (_input != null) _input.EnableInput();
        }

        private void OnDisable()
        {
            if (_input != null) _input.DisableInput();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleSteering();
            HandleLift();
        }

        private void HandleMovement()
        {
            float moveInput = _input.MoveInput;
            float targetSpeed = moveInput * _maxSpeed;

            //inertia logic
            float rate = (Mathf.Abs(moveInput) > 0.01f) ? _acceleration : _deceleration;
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, rate * Time.fixedDeltaTime);

            Vector3 velocity = transform.forward * _currentSpeed;

            // Preserving vertical velocity (Gravity)
            velocity.y = _rb.linearVelocity.y;
            _rb.linearVelocity = velocity;
        }

        private void HandleSteering()
        {
            float steerInput = _input.SteerInput;

            // 1. Calculate target wheel angle
            float targetAngle = steerInput * _maxSteerAngle;
            _currentSteerAngle = Mathf.Lerp(_currentSteerAngle, targetAngle, _steerSpeed * Time.fixedDeltaTime);

            // 2. Rotate the Rigidbody only if moving or steering intent exists
           //VERIFY if (Mathf.Abs(_currentSpeed) > 0.1f || Mathf.Abs(steerInput) > 0.1f)
                if ( Mathf.Abs(steerInput) > 0.1f)
            {
                // Normalize steering factor (-1 to 1)
                float turnFactor = _currentSteerAngle / _maxSteerAngle;

                // Invert rotation when reversing for natural car-like feeling
                float direction = (_currentSpeed >= 0) ? 1f : -1f;

                float rotationAmount = turnFactor * _rotationSpeed * direction * Time.deltaTime;

                Quaternion turnRotation = Quaternion.Euler(0f, rotationAmount, 0f);
                
                _rb.MoveRotation(_rb.rotation * turnRotation);
            }

            UpdateVisuals();
        }

        public void ResetVehicle()
        {
            _currentSpeed = 0f;
            _currentSteerAngle = 0f;

            if (_rb != null)
            {
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;

                // clear sleeping state to ensure physics wake up correctly
                _rb.WakeUp();
            }

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (_rearAxle != null)
            {
                _rearAxle.localRotation = Quaternion.Euler(0, _currentSteerAngle, 0);
            }

            if (_steeringWheel != null)
            {
                // Multiplier 3 for visual effect (steering wheel turns more than wheels)
                _steeringWheel.localRotation = Quaternion.Euler(0, -_currentSteerAngle * 3, 0);
            }
        }

        private void HandleLift()
        {
            if (_liftMechanism == null) return;
            if (Mathf.Abs(_input.ForkLiftInput) < 0.1f) return;

            float currentY = _liftMechanism.localPosition.y;
            float nextY = currentY + (_input.ForkLiftInput * _liftSpeed * Time.deltaTime);

            nextY = Mathf.Clamp(nextY, _minLiftHeight, _maxLiftHeight);

            _liftMechanism.localPosition = new Vector3(
                _liftMechanism.localPosition.x,
                nextY,
                _liftMechanism.localPosition.z
            );
        }
    }
}