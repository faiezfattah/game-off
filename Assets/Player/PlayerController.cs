using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Private Reference")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _rotatingContainer;
    [SerializeField] private GameObject _wallCheck;
    [SerializeField] private LayerMask _ground;

    [Header("Public Reference")]
    public PlayerVisualizer visual;
    public Rigidbody rb;
    public PlayerStamina stamina;
    public PlayerHealth health;

    [Header("Movement Settings")]
    public float walkSpeed = 1f;
    public float runSpeed = 3f;

    [Space(10)]
    public float dashForce = 100f;
    public float dashDuration = 1f;
    public float dashCooldown = 1f;
    public float dashVelocityRetention = 0.75f;

    [Space(10)]
    public float jumpForce = 10f;
    public float jumpDuration = 1f;
    public float jumpBuffer = 0.5f;

    [Space(10)]
    public float fallForce = -100f;
    public float fallSlowForce = -50f;
    public float aircControl = 5f;

    [Space(10)]
    public float coyoteTime = 0.5f;
    public float groundCheckDistance = 0.1f;
    public float wallCheckRadius = 1f;

    [Space(10)]
    public string currentState;
    
    [Header("Inputs and derived states")]
    public float dirHorizontal;
    public float dirVertical;
    public Vector2 mousePos;

    [Space(10)]
    public bool isRunPressed;

    [Space(10)]
    public bool isJumpPressed;
    public bool haveJump;
    public bool isJumpQueued;
    public bool isCoyote;

    [Space(10)]
    public bool isDashPressed;
    public bool isDashAim;
    public bool isDashQueued;
    public bool isDashCoolingDown;

    [Space(10)]
    public bool isGrounded;

    [Space(10)]
    public bool isWalled;
    public bool isWallGrabbedPressed;
    public bool isWallGrabQueued;

    [Space(10)]
    public bool isFrenzy;

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
            _rotatingContainer.transform.rotation = Quaternion.Euler(new Vector3 (0, dirHorizontal > 0? 0 : 180f,0));
        } 

        //isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, _ground);
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * playerHalfHeight, wallCheckRadius, _ground);
        visual.radius = wallCheckRadius;
        isWalled = Physics.CheckSphere(_wallCheck.transform.position, wallCheckRadius, _ground);

        #region wall grab check
        isWallGrabQueued = isWalled && (isWallGrabbedPressed/* || dirHorizontal != 0*/) && stamina.TryReduce(stamina.wallGrabRateCost*Time.deltaTime);
        #endregion

        #region jump check
        if (isGrounded || isWallGrabQueued || isFrenzy) {
            _coyoteTimer = coyoteTime;
            haveJump = true;
        }
        else if (isJumpQueued) {
            _coyoteTimer = 0; 
        }

        _coyoteTimer = Mathf.Max(_coyoteTimer - Time.deltaTime, 0);
        _jumpBufferTimer = Mathf.Max(_jumpBufferTimer - Time.deltaTime, 0);

        isCoyote = _coyoteTimer > 0;
        isJumpQueued = _jumpBufferTimer > 0 && (haveJump || isCoyote) && (isFrenzy || stamina.TryReduce(stamina.jumpCost));

        if (isJumpQueued) { 
            haveJump = false;
            isFrenzy = false;
        };
        #endregion

        #region dash check
        isDashQueued = _dashTimer <= 0 && isDashPressed && (isFrenzy || stamina.TryReduce(stamina.dashCost));

        if (isDashPressed && _dashTimer <= 0) {
            _dashTimer = dashCooldown;
            isFrenzy = false;
        }
        _dashTimer = Mathf.Max(_dashTimer - Time.deltaTime, 0);
        #endregion
    }
    private void OnFrenzy() {
       if (!health.TryReduce(health.frenzyCost)) return;
       isFrenzy = true;
    }
    #region input listeners
    private void OnMove(int value) {
        dirHorizontal = value;
    }
    private void OnJump(bool value) {
        isJumpPressed = value;
        if (value) { _jumpBufferTimer = jumpBuffer; }
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
        _inputReader.FrenzyEvent += OnFrenzy;
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
        _inputReader.FrenzyEvent -= OnFrenzy;
    }

    #endregion
}
