using UnityEngine;
using UnityEngine.Events;

namespace Acreos.ForkliftSim.Inputs
{
    public abstract class InputReader : ScriptableObject
    {
        // Polling Values (Ranges from -1 to 1)
        public abstract float MoveInput { get; }
        public abstract float SteerInput { get; }
        public abstract float ForkLiftInput { get; }

        // Event-Driven Actions
        public event UnityAction OnPauseEvent;
        protected void RaisePauseEvent() => OnPauseEvent?.Invoke();

        public abstract void EnableInput();
        public abstract void DisableInput();
    }
}