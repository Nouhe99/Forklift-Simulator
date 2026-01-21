using UnityEngine;

namespace Acreos.ForkliftSim.Core
{
    public class BriefingState : BaseGameState
    {
        private readonly int _missionIndex;
        private readonly BaseGameState _nextState;

        public BriefingState(GameManager ctx, int missionIndex, BaseGameState nextState) : base(ctx)
        {
            _missionIndex = missionIndex;
            _nextState = nextState;
        }

        public override void Enter()
        {
            // Freeze Vehicle
            if (_ctx.Forklift != null)
            {
                _ctx.Forklift.enabled = false;
                _ctx.Forklift.ResetVehicle();
            }

            // Freeze Camera & Show Cursor
            if (_ctx.MainCamera != null)
            {
                _ctx.MainCamera.SetInputActive(false);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Show UI
            if (_ctx.BriefingUI != null)
            {
                _ctx.BriefingUI.OnBriefingCompleted.AddListener(OnBriefingFinished);
                _ctx.BriefingUI.StartMissionBriefing(_missionIndex);
            }
            else
            {
                // Fallback if UI is missing
                OnBriefingFinished();
            }
        }

        private void OnBriefingFinished()
        {
            _ctx.SwitchState(_nextState);
        }

        public override void Exit()
        {
            if (_ctx.BriefingUI != null)
            {
                _ctx.BriefingUI.OnBriefingCompleted.RemoveListener(OnBriefingFinished);
            }
        }

        public override void Tick() { }
        public override void FixedTick() { }
    }
}