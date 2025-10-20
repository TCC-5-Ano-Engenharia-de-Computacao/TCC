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
        public UnityEvent<string, bool> inputPerformedEvent;

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

            // Weak Punch
            playerInputActions.Default.WeakPunch.performed += HandleWeakPunchPerformed;
            playerInputActions.Default.WeakPunch.canceled += HandleWeakPunchCanceled;
            
            // Strong Punch
            playerInputActions.Default.StrongPunch.performed += HandleStrongPunchPerformed;
            playerInputActions.Default.StrongPunch.canceled += HandleStrongPunchCanceled;

            // Kick
            playerInputActions.Default.Kick.performed += HandleKickPerformed;
            playerInputActions.Default.Kick.canceled += HandleKickCanceled;

            // Special
            playerInputActions.Default.Special.performed += HandleSpecialPerformed;
            playerInputActions.Default.Special.canceled += HandleSpecialCanceled;
            
            // Jump
            playerInputActions.Default.Jump.performed += HandleJumpPerformed;
            playerInputActions.Default.Jump.canceled += HandleJumpCanceled;

            // Block
            playerInputActions.Default.Block.performed += HandleBlockPerformed;
            playerInputActions.Default.Block.canceled += HandleBlockCanceled;

            // Pause
            playerInputActions.Default.Pause.performed += ctx =>
            {
                // Toggle pause menu
                var pauseController = FindFirstObjectByType<RoundControl.PauseController>();
                if (pauseController != null)
                    pauseController.TogglePause();
            };
        }

        private void OnDisable()
        {
            // Weak Punch
            playerInputActions.Default.WeakPunch.performed -= HandleWeakPunchPerformed;
            playerInputActions.Default.WeakPunch.canceled -= HandleWeakPunchCanceled;

            // Strong Punch
            playerInputActions.Default.StrongPunch.performed -= HandleStrongPunchPerformed;
            playerInputActions.Default.StrongPunch.canceled -= HandleStrongPunchCanceled;
            
            // Kick
            playerInputActions.Default.Kick.performed -= HandleKickPerformed;
            playerInputActions.Default.Kick.canceled -= HandleKickCanceled;
            
            // Special
            playerInputActions.Default.Special.performed -= HandleSpecialPerformed;
            playerInputActions.Default.Special.canceled -= HandleSpecialCanceled;

            // Jump
            playerInputActions.Default.Jump.performed -= HandleJumpPerformed;
            playerInputActions.Default.Jump.canceled -= HandleJumpCanceled;

            // Block
            playerInputActions.Default.Block.performed -= HandleBlockPerformed;
            playerInputActions.Default.Block.canceled -= HandleBlockCanceled;

            playerInputActions.Disable();
        }

        // ------------------------------------------------------- HANDLERS
        private void HandleWeakPunchPerformed(InputAction.CallbackContext ctx)
        {
            //Debug.Log("[C] - Weak Punch");
            inputPerformedEvent?.Invoke("weakPunch", true);
        }
        
        private void HandleWeakPunchCanceled(InputAction.CallbackContext ctx)
        {
            //Debug.Log("Strong Punch canceled");
        }
        
        private void HandleStrongPunchPerformed(InputAction.CallbackContext ctx)
        {
            //Debug.Log("[V] - Strong Punch");
            inputPerformedEvent?.Invoke("strongPunch", true);
        }
        
        private void HandleStrongPunchCanceled(InputAction.CallbackContext ctx)
        {
            //Debug.Log("Weak Punch canceled");
        }

        private void HandleKickPerformed(InputAction.CallbackContext ctx)
        {
            //Debug.Log("[B] - Kick");
            inputPerformedEvent?.Invoke("kick", true);
        }

        private void HandleKickCanceled(InputAction.CallbackContext ctx)
        {
            //Debug.Log("Kick canceled");
        }
        
        private void HandleSpecialPerformed(InputAction.CallbackContext ctx)
        {
            //Debug.Log("[N] - Special");
            inputPerformedEvent?.Invoke("special", true);
        }

        private void HandleSpecialCanceled(InputAction.CallbackContext ctx)
        {
            //Debug.Log("Special canceled");
        }

        private void HandleJumpPerformed(InputAction.CallbackContext ctx)
        {
            //Debug.Log("[Space] - Jump");
            inputPerformedEvent?.Invoke("jump", true);
        }

        private void HandleJumpCanceled(InputAction.CallbackContext ctx)
        {
            //Debug.Log("Jump canceled");
        }

        private void HandleBlockPerformed(InputAction.CallbackContext ctx)
        {
            //Debug.Log("Block");
            inputPerformedEvent?.Invoke("block", true);
        }

        private void HandleBlockCanceled(InputAction.CallbackContext ctx)
        {
            //Debug.Log("Block canceled");
            inputPerformedEvent?.Invoke("block", false);
        }

        public void ForceDisablePlayerInput()
        {
            playerInputActions.Default.Disable();
        }

        public void ForceEnablePlayerInput()
        {
            playerInputActions.Default.Enable();
        }
}
}
