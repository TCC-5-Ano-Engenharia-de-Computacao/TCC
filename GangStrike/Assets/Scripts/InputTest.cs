using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public PlayerInputActions playerInputActions;
    
    private PlayerInput _playerInput;


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        _playerInput.actions = playerInputActions.asset;
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
    }
    
    private void HandlePunchCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Punch canceled");
    }
    
    private void HandleKickPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("[V] - Kick");
    }
    
    private void HandleKickCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Kick canceled");
    }
}
