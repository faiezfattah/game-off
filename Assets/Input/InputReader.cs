using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, Input.IDefaultActions {

    private Input _inputActions;

    public event UnityAction RunEvent;
    public event UnityAction<bool> JumpEvent;
    public event UnityAction<bool> DashEvent;
    public event UnityAction<bool> DashHoldEvent;

    /// <summary>
    /// return mouse directional int
    /// </summary>
    public event UnityAction<int> MoveEvent;

    /// <summary>
    /// return mouse location vector2
    /// </summary>
    public event UnityAction<Vector2> MousePositionEvent;

    private void OnEnable() {
        if (_inputActions == null) {
            _inputActions = new Input();
            _inputActions.Default.SetCallbacks(this);
        }

        EnableInput();
    }

    private void OnDisable() {
        DisableInput();
    }

    public void OnRun(InputAction.CallbackContext ctx) {
        RunEvent?.Invoke();
    }
    public void OnMove(InputAction.CallbackContext ctx) {        
        MoveEvent?.Invoke(Convert.ToInt32(ctx.ReadValue<float>()));
    }
    public void OnJump(InputAction.CallbackContext ctx) {
        JumpEvent.Invoke(ctx.performed);
    }
    public void OnMousePosition(InputAction.CallbackContext ctx) {
        MousePositionEvent?.Invoke(ctx.ReadValue<Vector2>());
    }
    public void OnDash(InputAction.CallbackContext ctx) {
        DashEvent?.Invoke(ctx.canceled);
        DashHoldEvent?.Invoke(ctx.performed);
    }

    private void EnableInput() {
        _inputActions.Default.Enable();
    }

    private void DisableInput() {
        _inputActions.Default.Disable();
    }

}
