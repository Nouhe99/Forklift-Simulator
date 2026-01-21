using UnityEngine;
using Acreos.ForkliftSim.Gameplay;

namespace Acreos.ForkliftSim.Core
{
    public class DrivingState : BaseGameState
    {
        private Vector3 _startPosition;
        private Quaternion _startRotation;

        public DrivingState(GameManager ctx) : base(ctx) { }

        public override void Enter()
        {
            Debug.Log("<color=orange>--- MISSION 1 : DRIVING ---</color>");

            // 1. Enable Vehicle & Store Spawn Point
            if (_ctx.Forklift != null)
            {
                _ctx.Forklift.enabled = true;
                _startPosition = _ctx.Forklift.transform.position;
                _startRotation = _ctx.Forklift.transform.rotation;
            }

            // 2. Enable Camera
            if (_ctx.MainCamera != null)
            {
                _ctx.MainCamera.SetInputActive(true);
            }

            // 3. Subscribe to Events
            if (_ctx.ParkingZone != null)
                _ctx.ParkingZone.OnParked.AddListener(OnSuccess);

            Obstacle.OnObstacleHit += OnFail;
        }

        private void OnSuccess()
        {
            Debug.Log("<color=green> Parking Successful.</color>");

            // Environment Swap
            if (_ctx.Mission1Group != null) _ctx.Mission1Group.SetActive(false);
            if (_ctx.Mission2Group != null) _ctx.Mission2Group.SetActive(true);

            // Transition: Go to Briefing 2 -> Then TrainingState
            var trainingState = new TrainingState(_ctx);
            var transition = new BriefingState(_ctx, 1, trainingState);

            _ctx.SwitchState(transition);
        }

        private void OnFail()
        {
            Debug.Log("<color=red> FAILED: Obstacle hit.</color>");

            if (_ctx.Forklift != null)
            {
                _ctx.Forklift.transform.SetPositionAndRotation(_startPosition, _startRotation);
                _ctx.Forklift.ResetVehicle();
            }
        }

        public override void Exit()
        {
            if (_ctx.ParkingZone != null)
                _ctx.ParkingZone.OnParked.RemoveListener(OnSuccess);

            Obstacle.OnObstacleHit -= OnFail;
        }

        public override void Tick() { }
        public override void FixedTick() { }
    }
}