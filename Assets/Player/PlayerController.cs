using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _ground;


    [Header("Movement Settings")]
    public float walkSpeed = 10f;
    public float runSpeed = 8f;
    public float dashForce = 100f;
    public float dashDuration = 1f;
    public float jumpForce = 10f;
    public float jumpDuration = 1f;
    public float jumpBuffer = 0.5f;
    public float fallForce = -100f;
    public float fallFastForce = -200f;
    public float aircControl = 5f;
    public float coyoteTime = 0.5f;
    public float groundCheckDistance = 0.1f;

    [HideInInspector]
    public Rigidbody rb;
    public float dir;
    public Vector2 dashDir;
    public bool isRunPressed;
    public bool isJumpPressed;
    public bool isJumpQueue;
    public bool isCoyote;
    public bool isDashPressed;
    public bool isGrounded;

    private float playerHalfHeight = 0.5f;
    private Vector3 _linearVelocity;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    void Start(){
        playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
        groundCheckDistance += playerHalfHeight;
        rb = GetComponent<Rigidbody>();
        _linearVelocity = rb.linearVelocity;
    }
    void Update() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, _ground);

        if (!isGrounded) {
            _coyoteTimer += Time.deltaTime;
            isCoyote = _coyoteTimer < coyoteTime;
        }
        else { _coyoteTimer = 0; }


        if (isJumpPressed) {
            _jumpBufferTimer = jumpBuffer;
        }

        _jumpBufferTimer = Mathf.Max(_jumpBufferTimer - Time.deltaTime, 0);
        isJumpQueue = _jumpBufferTimer > 0;
    }
    private void OnMove(int value) {
        dir = value;
    }
    private void OnJump(bool value) {
        isJumpPressed = value;
    }
    private void OnDash(bool value) {
        isDashPressed = value;
    }
    private void MousePosition(Vector2 value) {
        dashDir = value;
    }
    private void OnRun(bool value) {
        isRunPressed = value;
    }
    private void OnEnable() {
        _inputReader.MoveEvent += OnMove;
        _inputReader.JumpEvent += OnJump;
        _inputReader.DashEvent += OnDash;
        _inputReader.RunEvent += OnRun;
        _inputReader.MousePositionEvent += MousePosition;

    }
    private void OnDisable() {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJump;
        _inputReader.DashEvent -= OnDash;
        _inputReader.RunEvent -= OnRun;
        _inputReader.MousePositionEvent -= MousePosition;
    }


}
