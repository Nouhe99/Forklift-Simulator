using UnityEngine;
using UnityEngine.Events;

namespace Acreos.ForkliftSim.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class DropZone : MonoBehaviour
    {
        [Header("Validation Settings")]
        [SerializeField] private float _validationTime = 2.0f;

        [SerializeField] private float _maxVelocityToValidate = 0.1f;

        [Header("Events")]
        public UnityEvent OnObjectiveCompleted;

        private Palette _currentPallet;
        private float _timer;
        private bool _isCompleted = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_isCompleted) return;

            if (other.attachedRigidbody != null)
            {
                Palette p = other.attachedRigidbody.GetComponent<Palette>();
                if (p != null)
                {
                    _currentPallet = p;
                    _timer = 0f;
                    Debug.Log($"[DropZone] Palette detected: {p.name}");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isCompleted || other.attachedRigidbody == null) return;

            Palette p = other.attachedRigidbody.GetComponent<Palette>();

            // If the exiting pallet is the one we were tracking
            if (p != null && p == _currentPallet)
            {
                _currentPallet = null;
                _timer = 0f;
            }
        }

        private void Update()
        {
            if (_currentPallet == null || _isCompleted) return;

            // Velocity Check: Is the pallet actually placed on the ground?
            float speed = _currentPallet.GetComponent<Rigidbody>().linearVelocity.magnitude;

            if (speed < _maxVelocityToValidate)
            {
                _timer += Time.deltaTime;
                if (_timer >= _validationTime)
                {
                    ValidateObjective();
                }
            }
            else
            {
                // Reset timer if pallet moves (e.g. player is adjusting position)
                _timer = 0f;
            }
        }

        private void ValidateObjective()
        {
            _isCompleted = true;
            Debug.Log("<color=green>[DropZone] OBJECTIVE COMPLETED!</color>");
            OnObjectiveCompleted?.Invoke();

            
        }
    }
}