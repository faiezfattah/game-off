using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask ground;

    private float playerHalfHeight = 0.5f;

    [Header("Movement Settings")]
    public float walkSpeed = 10f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;
    public float fallSpeed = -100f;
    public float groundCheckDistance = 0.1f;
    public float coyoteJumpTime = 0.5f;

    [HideInInspector]
    public Rigidbody rb;
    public float dir;
    public Vector2 dashDir;
    public bool isRunPressed;
    public bool isJumpPressed;
    public bool isDashPressed;
    public bool isJumping;
    public bool isGrounded;

    private Vector3 _linearVelocity;
    void Start(){
        playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
        groundCheckDistance += playerHalfHeight;
        rb = GetComponent<Rigidbody>();
        _linearVelocity = rb.linearVelocity;

    }
    void Update() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, ground);
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
    private void OnEnable() {
        _inputReader.MoveEvent += OnMove;
        _inputReader.JumpEvent += OnJump;
        _inputReader.DashEvent += OnDash;
        _inputReader.MousePositionEvent += MousePosition;
    }
    private void OnDisable() {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJump;
        _inputReader.DashEvent -= OnDash;
        _inputReader.MousePositionEvent -= MousePosition;
    }


}
