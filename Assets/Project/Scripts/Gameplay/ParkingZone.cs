using UnityEngine;
using UnityEngine.Events;
using Acreos.ForkliftSim.Vehicle;

namespace Acreos.ForkliftSim.Gameplay
{
    public class ParkingZone : MonoBehaviour
    {
        public UnityEvent OnParked;

        private bool _isCompleted = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_isCompleted) return;

            // Check for Forklift controller in parent
            if (other.GetComponentInParent<ForkliftController>() != null)
            {
                ValidateParking();
            }
        }

        private void ValidateParking()
        {
            _isCompleted = true;
            Debug.Log("<color=green>[DropZone] OBJECTIVE COMPLETED!</color>");

            OnParked?.Invoke();
        }
    }
}