using UnityEngine;
using Acreos.ForkliftSim.Vehicle;

namespace Acreos.ForkliftSim.Gameplay
{
    public class Obstacle : MonoBehaviour
    {
        // Global static event for simple failure notification
        public static event System.Action OnObstacleHit;

        private void OnCollisionEnter(Collision collision)
        {
            // Only trigger if hit by the Forklift
            if (collision.gameObject.GetComponentInParent<ForkliftController>())
            {
                OnObstacleHit?.Invoke();
            }
        }
    }
}