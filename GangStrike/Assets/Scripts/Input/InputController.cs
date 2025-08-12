using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Input
{
    public class InputController : MonoBehaviour
    {
        public PlayerInputActions playerInputActions;
        public UnityEvent<string> inputPerformedEvent;
    
        [SerializeField] private PlayerInput playerInput;
    
        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
            playerInput.actions = playerInputActions.asset;
        }

        private void OnEnable()
        {
            playerInputActions.Default.Punch.performed += HandlePunchPerformed;
            playerInputActions.Default.Punch.canceled += HandlePunchCanceled;
            playerInputActions.Default.Kick.performed += HandleKickPerformed;
            playerInputActions.Default.Kick.canceled += HandleKickCanceled;
        }

        private void OnDisable()
        {
            playerInputActions.Default.Punch.performed -= HandlePunchPerformed;
            playerInputActions.Default.Punch.canceled -= HandlePunchCanceled;
            playerInputActions.Default.Kick.performed -= HandleKickPerformed;
            playerInputActions.Default.Kick.canceled -= HandleKickCanceled;
        }
    
        private void HandlePunchPerformed(InputAction.CallbackContext ctx)
        {
            Debug.Log("[C] - Punch");
            inputPerformedEvent.Invoke("attack");
        }
    
        private void HandlePunchCanceled(InputAction.CallbackContext ctx)
        {
            Debug.Log("Punch canceled");
        }
    
        private void HandleKickPerformed(InputAction.CallbackContext ctx)
        {
            Debug.Log("[V] - Kick");
            inputPerformedEvent.Invoke("kick");
        }
    
        private void HandleKickCanceled(InputAction.CallbackContext ctx)
        {
            Debug.Log("Kick canceled");
        }
    }
}
