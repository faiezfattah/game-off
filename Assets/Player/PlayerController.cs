using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _rotatingContainer;
    [SerializeField] private GameObject _wallCheck;
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
    public float dashVelocityRetention = 0.75f;
    public float jumpForce = 10f;
    public float jumpDuration = 1f;
    public float jumpBuffer = 0.5f;
    public float fallForce = -100f;
    public float fallSlowForce = -50f;
    public float aircControl = 5f;
    public float coyoteTime = 0.5f;
    public float groundCheckDistance = 0.1f;
    public float wallCheckRadius = 1f;
    public string currentState;

    [Header("Inputs and derived states")]
    public float dirHorizontal;
    public float dirVertical;
    public Vector2 mousePos;
    public bool isRunPressed;
    public bool isJumpPressed;
    public bool haveJump;
    public bool isJumpQueued;
    public bool isCoyote;
    public bool isDashPressed;
    public bool isDashAim;
    public bool isDashQueued;
    public bool isDashCoolingDown;
    public bool isGrounded;
    public bool isWalled;
    public bool isWallGrabbedPressed;
    public bool isWallGrabQueued;

    private float playerHalfHeight = 0.5f;
    private Vector3 _linearVelocity;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private float _dashTimer;
    void Start(){
        playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
        groundCheckDistance += playerHalfHeight;
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!visual) visual = GetComponent<PlayerVisualizer>();
        if (_ground == 0) _ground = LayerMask.GetMask("Ground");
        _linearVelocity = rb.linearVelocity;
    }
    void Update() {
        if (dirHorizontal != 0) {
            _rotatingContainer.transform.rotation = Quaternion.Euler(new Vector3 (0,dirHorizontal > 0? 0 : 180f,0));
        } 

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, _ground);
        visual.radius = wallCheckRadius;
        isWalled = Physics.OverlapSphere(_wallCheck.transform.position, wallCheckRadius, _ground).Length > 0;

        #region wall grab check
        isWallGrabQueued = isWalled && (isWallGrabbedPressed/* || dirHorizontal != 0*/);
        #endregion

        #region jump check
        if (isGrounded || isWallGrabQueued) {
            _coyoteTimer = coyoteTime;
            haveJump = true;
        }

        if (isJumpPressed) {
            _jumpBufferTimer = jumpBuffer;
        }
        if (!isGrounded && isJumpQueued) {
            _coyoteTimer = 0;
        }
        _coyoteTimer = Mathf.Max(_coyoteTimer - Time.deltaTime, 0);
        _jumpBufferTimer = Mathf.Max(_jumpBufferTimer - Time.deltaTime, 0);

        isCoyote = _coyoteTimer > 0;
        isJumpQueued = _jumpBufferTimer > 0 && (haveJump || isCoyote);
        if (isJumpQueued) haveJump = false;


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
        dirHorizontal = value;
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
    private void OnWallGrab(bool value) {
        isWallGrabbedPressed = value;
    }
    private void OnWallSlide(int value) {
        dirVertical = value;
    }
    private void OnEnable() {
        _inputReader.MoveEvent += OnMove;
        _inputReader.JumpEvent += OnJump;
        _inputReader.DashStartEvent += OnDash;
        _inputReader.RunEvent += OnRun;
        _inputReader.MousePositionEvent += MousePosition;
        _inputReader.DashAimEvent += OnDashAim;
        _inputReader.WallGrabEvent += OnWallGrab;
        _inputReader.SlideEvent += OnWallSlide;
    }
    private void OnDisable() {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJump;
        _inputReader.DashStartEvent -= OnDash;
        _inputReader.RunEvent -= OnRun;
        _inputReader.MousePositionEvent -= MousePosition;
        _inputReader.DashAimEvent -= OnDashAim;
        _inputReader.WallGrabEvent -= OnWallGrab;
        _inputReader.SlideEvent -= OnWallSlide;
    }

    #endregion
}
