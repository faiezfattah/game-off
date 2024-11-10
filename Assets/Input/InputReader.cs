using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, Input.IDefaultActions {

    private Input _inputActions;

    public event UnityAction<bool> RunEvent;
    public event UnityAction<bool> WallGrabEvent;
    public event UnityAction<bool> JumpEvent;
    public event UnityAction<bool> DashStartEvent;
    public event UnityAction<bool> DashAimEvent;
    public event UnityAction FrenzyEvent;

    /// <summary>
    /// return mouse directional int
    /// </summary>
    public event UnityAction<int> MoveEvent;
    public event UnityAction<int> SlideEvent;

    /// <summary>
    /// return mouse location vector2. Screen space
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
        RunEvent?.Invoke(ctx.performed);
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
        DashStartEvent?.Invoke(ctx.performed);
        DashAimEvent?.Invoke(ctx.canceled);
    }
    public void OnWallGrab(InputAction.CallbackContext ctx) {
        WallGrabEvent?.Invoke(ctx.performed);
    }
    public void OnSlide(InputAction.CallbackContext ctx) {
        SlideEvent?.Invoke(Convert.ToInt32(ctx.ReadValue<float>()));
    }
    public void OnFrenzy(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            FrenzyEvent?.Invoke();
        }
    }
    private void EnableInput() {
        _inputActions.Default.Enable();
    }

    private void DisableInput() {
        _inputActions.Default.Disable();
    }
}
