using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _ground;

    [Header("Public Reference")]
    public PlayerVisualizer visual;
    public Rigidbody rb;

    [Header("Movement Settings")]
    public float walkSpeed = 10f;
    public float runSpeed = 8f;
    public float dashForce = 100f;
    public float dashDuration = 1f;
    public float dashCooldown = 1f;
    public float jumpForce = 10f;
    public float jumpDuration = 1f;
    public float jumpBuffer = 0.5f;
    public float fallForce = -100f;
    public float fallFastForce = -200f;
    public float aircControl = 5f;
    public float coyoteTime = 0.5f;
    public float groundCheckDistance = 0.1f;
    public string currentState;

    [Header("Inputs and derived states")]
    public float dir;
    public Vector2 mousePos;
    public Vector2 aimDir; // Debugging purposes. TODO: remove later or find a better solution (drived from state script)
    public bool isRunPressed;
    public bool isJumpPressed;
    public bool isJumpQueued;
    public bool isCoyote;
    public bool isDashPressed;
    public bool isDashAim;
    public bool isDashQueued;
    public bool isDashCoolingDown;
    public bool isGrounded;

    private float playerHalfHeight = 0.5f;
    private Vector3 _linearVelocity;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private float _dashTimer;
    void Start(){
        playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
        groundCheckDistance += playerHalfHeight;
        rb = GetComponent<Rigidbody>();
        _linearVelocity = rb.linearVelocity;
    }
    void Update() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, _ground);

        #region jump check
        if (isGrounded) {
            _coyoteTimer = coyoteTime;
        }

        if (isJumpPressed) {
            _jumpBufferTimer = jumpBuffer;
        }

        _coyoteTimer = Mathf.Max(_coyoteTimer - Time.deltaTime, 0);
        _jumpBufferTimer = Mathf.Max(_jumpBufferTimer - Time.deltaTime, 0);

        isCoyote = _coyoteTimer > 0;
        isJumpQueued = _jumpBufferTimer > 0 && (isCoyote || isGrounded);
        #endregion

        #region dash check
        isDashQueued = _dashTimer <= 0 && isDashPressed;

        if (isDashPressed && _dashTimer <= 0) {
            _dashTimer = dashCooldown;
        }
        _dashTimer = Mathf.Max(_dashTimer - Time.deltaTime, 0);
        #endregion
    }
    #region input listeners
    private void OnMove(int value) {
        dir = value;
    }
    private void OnJump(bool value) {
        isJumpPressed = value;
    }
    private void OnDash(bool value) {
        isDashPressed = value;
    }
    private void OnDashAim(bool value) {
        isDashAim = value;
    }
    private void MousePosition(Vector2 value) {
        mousePos = value;
    }
    private void OnRun(bool value) {
        isRunPressed = value;
    }
    private void OnEnable() {
        _inputReader.MoveEvent += OnMove;
        _inputReader.JumpEvent += OnJump;
        _inputReader.DashStartEvent += OnDash;
        _inputReader.RunEvent += OnRun;
        _inputReader.MousePositionEvent += MousePosition;
        _inputReader.DashAimEvent += OnDashAim;
    }
    private void OnDisable() {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJump;
        _inputReader.DashStartEvent -= OnDash;
        _inputReader.RunEvent -= OnRun;
        _inputReader.MousePositionEvent -= MousePosition;
        _inputReader.DashAimEvent += OnDashAim;
    }

    #endregion
}
