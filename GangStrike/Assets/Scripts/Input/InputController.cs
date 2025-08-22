using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Input
{
    /// <summary>
    /// Mapeia Punch, Kick e Jump usando o novo Input System.
    /// Dispara um UnityEvent&lt;string&gt; quando o input é executado.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        public PlayerInputActions playerInputActions;
        public UnityEvent<string> inputPerformedEvent;

        [SerializeField] private PlayerInput playerInput;

        // ------------------------------------------------------- LIFECYCLE
        private void Awake()
        {
            playerInputActions = new PlayerInputActions();

            // Garante a referência ao PlayerInput no inspector ou no mesmo GameObject
            if (playerInput == null)
                playerInput = GetComponent<PlayerInput>();

            // Conecta o asset gerado pelo Input Actions ao PlayerInput
            playerInput.actions = playerInputActions.asset;
        }

        private void OnEnable()
        {
            playerInputActions.Enable();

            // Punch
            playerInputActions.Default.Punch.performed  += HandlePunchPerformed;
            playerInputActions.Default.Punch.canceled   += HandlePunchCanceled;

            // Kick
            playerInputActions.Default.Kick.performed   += HandleKickPerformed;
            playerInputActions.Default.Kick.canceled    += HandleKickCanceled;

            // Jump
            playerInputActions.Default.Jump.performed   += HandleJumpPerformed;
            playerInputActions.Default.Jump.canceled    += HandleJumpCanceled;
        }

        private void OnDisable()
        {
            // Punch
            playerInputActions.Default.Punch.performed  -= HandlePunchPerformed;
            playerInputActions.Default.Punch.canceled   -= HandlePunchCanceled;

            // Kick
            playerInputActions.Default.Kick.performed   -= HandleKickPerformed;
            playerInputActions.Default.Kick.canceled    -= HandleKickCanceled;

            // Jump
            playerInputActions.Default.Jump.performed   -= HandleJumpPerformed;
            playerInputActions.Default.Jump.canceled    -= HandleJumpCanceled;

            playerInputActions.Disable();
        }

        // ------------------------------------------------------- HANDLERS
        private void HandlePunchPerformed(InputAction.CallbackContext ctx)
        {
            Debug.Log("[C] - Punch");
            inputPerformedEvent?.Invoke("attack");
        }

        private void HandlePunchCanceled(InputAction.CallbackContext ctx)
        {
            Debug.Log("Punch canceled");
        }

        private void HandleKickPerformed(InputAction.CallbackContext ctx)
        {
            Debug.Log("[V] - Kick");
            inputPerformedEvent?.Invoke("kick");
        }

        private void HandleKickCanceled(InputAction.CallbackContext ctx)
        {
            Debug.Log("Kick canceled");
        }

        private void HandleJumpPerformed(InputAction.CallbackContext ctx)
        {
            Debug.Log("[Space] - Jump");
            inputPerformedEvent?.Invoke("jump");
        }

        private void HandleJumpCanceled(InputAction.CallbackContext ctx)
        {
            Debug.Log("Jump canceled");
        }
    }
}
