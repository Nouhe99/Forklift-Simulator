using UnityEngine;

namespace Acreos.ForkliftSim.Core
{
    public class BootState : BaseGameState
    {
        public BootState(GameManager ctx) : base(ctx) { }

        public override void Enter()
        {
            // Placeholder for future initialization logic (Loading screens, login, etc.)
           
        }

        public override void Exit() { }
        public override void Tick() { }
        public override void FixedTick() { }
    }
}