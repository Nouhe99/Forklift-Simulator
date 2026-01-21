using UnityEngine;
using UnityEngine.InputSystem;

namespace Acreos.ForkliftSim.Inputs
{
    [CreateAssetMenu(fileName = "GameInputReader", menuName = "Acreos/Input/Game Input Reader")]
    public class GameInputReader : InputReader
    {
        [Header("Input Action References")]
        [SerializeField] private InputActionReference moveReference;
        [SerializeField] private InputActionReference steerReference;
        [SerializeField] private InputActionReference forkLiftReference;
        [SerializeField] private InputActionReference pauseReference;

        // --- Polling Implementation ---

        public override float MoveInput => ReadValueSafe(moveReference);
        public override float SteerInput => ReadValueSafe(steerReference);
        public override float ForkLiftInput => ReadValueSafe(forkLiftReference);

        // Helper to avoid null checks everywhere
        private float ReadValueSafe(InputActionReference refAction)
        {
            return (refAction != null) ? refAction.action.ReadValue<float>() : 0f;
        }

        // --- Lifecycle ---

        private void OnEnable()
        {
            if (pauseReference != null)
                pauseReference.action.performed += OnPausePerformed;
        }

        private void OnDisable()
        {
            DisableInput();
            if (pauseReference != null)
                pauseReference.action.performed -= OnPausePerformed;
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            RaisePauseEvent();
        }

        public override void EnableInput()
        {
            moveReference?.action.Enable();
            steerReference?.action.Enable();
            forkLiftReference?.action.Enable();
            pauseReference?.action.Enable();
        }

        public override void DisableInput()
        {
            moveReference?.action.Disable();
            steerReference?.action.Disable();
            forkLiftReference?.action.Disable();
            pauseReference?.action.Disable();
        }
    }
}