using UnityEngine;

namespace Acreos.ForkliftSim.Core
{
    public class WinState : BaseGameState
    {
        public WinState(GameManager ctx) : base(ctx) { }

        public override void Enter()
        {
            Debug.Log("<color=orange>--- VICTORY ---</color>");

            // Stop Vehicle
            if (_ctx.Forklift != null)
            {
                _ctx.Forklift.enabled = false;
                _ctx.Forklift.ResetVehicle();
            }

            // Show Victory Screen
            if (_ctx.BriefingUI != null)
            {
                _ctx.BriefingUI.ShowWinScreen(_ctx.WinMissionIndex);
            }

            // Disable Camera movement
            if (_ctx.MainCamera != null)
                _ctx.MainCamera.SetInputActive(false);
        }

        public override void Exit() { }
        public override void Tick() { }
        public override void FixedTick() { }
    }
}