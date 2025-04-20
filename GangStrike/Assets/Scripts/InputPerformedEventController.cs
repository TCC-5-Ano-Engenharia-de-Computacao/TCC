using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputPerformedEventController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    public UnityEvent<string> inputPerformedEvent;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _playerInput.actions = _playerInputActions.asset;
    }

    private void OnEnable()
    {
        _playerInputActions.Default.Move.performed += HandleMovePerformed;
        _playerInputActions.Default.Move.canceled += HandleMoveCanceled;
        _playerInputActions.Default.Jump.performed += HandleJumpPerformed;
        _playerInputActions.Default.Jump.canceled += HandleJumpCanceled;
        _playerInputActions.Default.Punch.performed += HandlePunchPerformed;
        _playerInputActions.Default.Punch.canceled += HandlePunchCanceled;
        _playerInputActions.Default.Kick.performed += HandleKickPerformed;
        _playerInputActions.Default.Kick.canceled += HandleKickCanceled;
    }

    private void OnDisable()
    {
        _playerInputActions.Default.Move.performed -= HandleMovePerformed;
        _playerInputActions.Default.Move.canceled -= HandleMoveCanceled;
        _playerInputActions.Default.Jump.performed -= HandleJumpPerformed;
        _playerInputActions.Default.Jump.canceled -= HandleJumpCanceled;
        _playerInputActions.Default.Punch.performed -= HandlePunchPerformed;
        _playerInputActions.Default.Punch.canceled -= HandlePunchCanceled;  
        _playerInputActions.Default.Kick.performed -= HandleKickPerformed;
        _playerInputActions.Default.Kick.canceled -= HandleKickCanceled;
    }

    private void HandleMovePerformed(InputAction.CallbackContext ctx)
    {
        float direction = ctx.ReadValue<float>();
        if (direction > 0)
        {
            Debug.Log($"[D] - Move right ({direction})");
            inputPerformedEvent.Invoke("Move_Right");
        }
        else
        {
            Debug.Log($"[A] - Move left ({direction})");
            inputPerformedEvent.Invoke("Move_Left");
        }
    }
    
    private void HandleMoveCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Move canceled");
    }

    private void HandleJumpPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("[W] - Jump");
        inputPerformedEvent.Invoke("Jump");
    }
    
    private void HandleJumpCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump canceled");
    }
    
    private void HandlePunchPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("[C] - Punch");
        inputPerformedEvent.Invoke("Punch");
    }
    
    private void HandlePunchCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Punch canceled");
    }
    
    private void HandleKickPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("[V] - Kick");
        inputPerformedEvent.Invoke("Kick");
    }
    
    private void HandleKickCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Kick canceled");
    }
}
