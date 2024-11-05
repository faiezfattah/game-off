using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, Input.IDefaultActions {

    private Input _inputActions;

    public event UnityAction RunEvent;
    public event UnityAction JumpEvent;

    /// <summary>
    /// return mouse directional int
    /// </summary>
    public event UnityAction<float> MoveEvent;

    /// <summary>
    /// return mouse location vector2
    /// </summary>
    public event UnityAction<Vector2> DashEvent;

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
        MoveEvent?.Invoke(ctx.ReadValue<float>());
    }
    public void OnJump(InputAction.CallbackContext ctx) {
        JumpEvent?.Invoke();
    }
    public void OnDash(InputAction.CallbackContext ctx) {
        DashEvent?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void EnableInput() {
        _inputActions.Default.Enable();
    }

    private void DisableInput() {
        _inputActions.Default.Disable();
    }
}
