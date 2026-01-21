using UnityEngine;
using Acreos.ForkliftSim.Gameplay;

namespace Acreos.ForkliftSim.Vehicle
{
    public class ForkStabilizer : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Height threshold under which the forks release the pallet.")]
        [SerializeField] private float _lockHeightThreshold = -0.36f;

        [Tooltip("Max distance between forks center and pallet center to allow locking.")]
        [SerializeField] private float _insideDistance = 2.2f;

        [SerializeField] private float _breakForce = 50000f;

        // Internal State
        private FixedJoint _currentJoint;
        private Palette _currentPallet;
        private bool _isLockingAllowed = false;

        private void Update()
        {
            // If no pallet is currently in contact or locked, do nothing
            if (_currentPallet == null) return;

            float forkHeight = transform.parent.localPosition.y;
            bool isJointCreated = _currentJoint != null;

            // --- LOCKING LOGIC ---
            // If forks go UP + Joint doesn't exist + Pallet is close enough
            if (forkHeight > _lockHeightThreshold && !isJointCreated && _isLockingAllowed)
            {
                // We double check distance to avoid locking a pallet that is slightly far
                float distance = GetFlatDistance(transform.position, _currentPallet.transform.position);

                if (distance <= _insideDistance)
                {
                    LockCargo(_currentPallet);
                }
            }

            // --- RELEASING LOGIC ---
            // If forks go DOWN + Joint exists
            if (forkHeight <= _lockHeightThreshold && isJointCreated)
            {
                ReleaseCargo();
            }
        }

        private float GetFlatDistance(Vector3 a, Vector3 b)
        {
            // Ignore Y axis for distance calculation (Top-down view distance)
            return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Only detect if we are not already carrying something
            if (_currentJoint != null) return;

            Palette p = collision.gameObject.GetComponentInParent<Palette>();
            if (p != null)
            {
                _currentPallet = p;
                _isLockingAllowed = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // If we leave the pallet without locking it, we must forget it
            if (_currentJoint == null && _currentPallet != null)
            {
                // Check if the exiting object is indeed the current pallet
                Palette p = collision.gameObject.GetComponentInParent<Palette>();
                if (p == _currentPallet)
                {
                    _currentPallet = null;
                    _isLockingAllowed = false;
                }
            }
        }

        void OnJointBreak(float breakForce)
        {
            Debug.LogWarning($"[ForkStabilizer] Joint broke due to excessive force: {breakForce}");
            ReleaseCargo(); 
        }

        private void LockCargo(Palette p)
        {
            Rigidbody targetRb = p.GetComponent<Rigidbody>();
            if (targetRb == null) return;

            _currentJoint = gameObject.AddComponent<FixedJoint>();
            _currentJoint.connectedBody = targetRb;
            _currentJoint.breakForce = _breakForce;

            // Optional: Reduce physics jitter
            _currentJoint.enableCollision = false;

            Debug.Log($"<color=cyan>[ForkStabilizer] Locked: {p.name}</color>");
        }

        private void ReleaseCargo()
        {
            if (_currentJoint != null)
            {
                Destroy(_currentJoint);
                _currentJoint = null;

                // IMPORTANT: We do NOT set _currentPallet to null here immediately
                // because we might still be touching it physically. 
                // OnCollisionExit will handle the cleanup if we drive away.

                Debug.Log($"<color=orange>[ForkStabilizer] Released.</color>");
            }
        }
    }
}