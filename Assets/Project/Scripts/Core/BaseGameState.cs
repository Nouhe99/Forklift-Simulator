namespace Acreos.ForkliftSim.Core
{
 
    public abstract class BaseGameState
    {
        protected GameManager _ctx;

        protected BaseGameState(GameManager ctx)
        {
            _ctx = ctx;
        }

        public abstract void Enter();
        public abstract void Tick();
        public abstract void FixedTick();
        public abstract void Exit();
    }
}