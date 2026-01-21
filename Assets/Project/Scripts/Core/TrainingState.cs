using UnityEngine;

namespace Acreos.ForkliftSim.Core
{
    public class TrainingState : BaseGameState
    {
        public TrainingState(GameManager ctx) : base(ctx) { }

        public override void Enter()
        {
            Debug.Log("<color=orange>--- MISSION 2 : FORKLIFT HANDLING ---</color>");

            if (_ctx.Forklift != null)
                _ctx.Forklift.enabled = true;

            if (_ctx.MainCamera != null)
                _ctx.MainCamera.SetInputActive(true);

            if (_ctx.TargetZone != null)
                _ctx.TargetZone.OnObjectiveCompleted.AddListener(OnVictory);
        }

        private void OnVictory()
        {
            _ctx.SwitchState(new WinState(_ctx));
        }

        public override void Exit()
        {
            if (_ctx.TargetZone != null)
                _ctx.TargetZone.OnObjectiveCompleted.RemoveListener(OnVictory);
        }

        public override void Tick() { }
        public override void FixedTick() { }
    }
}