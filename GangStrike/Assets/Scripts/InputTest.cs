using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;


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
        }
        else
        {
            Debug.Log($"[A] - Move left ({direction})");
        }
    }
    
    private void HandleMoveCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Move canceled");
    }

    private void HandleJumpPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("[W] - Jump");
    }
    
    private void HandleJumpCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump canceled");
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
