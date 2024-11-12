using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

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
    public PlayerMovementSettings settings;

    [Header("Movement QoL")]
    public float coyoteTime = 0.5f;
    public float groundCheckRadius = 0.1f;
    public float wallCheckRadius = 1f;

    [Space(10)]
    public string currentState;
    
    [Header("Inputs and derived states")]
    public float dirHorizontal;
    public float dirVertical;
    public Vector2 mousePos;

    [Space(10)]
    public bool isRunPressed;
    public bool isRunQueued;

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

    private float playerHalfHeight = 0.5f;
    private Vector3 _linearVelocity;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private float _dashTimer;
    void Start(){
        playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!visual) visual = GetComponent<PlayerVisualizer>();
        if (_ground == 0) _ground = LayerMask.GetMask("Ground");
        _linearVelocity = rb.linearVelocity;
    }
    void Update() {
        if (dirHorizontal != 0) {
            _rotatingContainer.transform.rotation = Quaternion.Euler(new Vector3 (0, dirHorizontal > 0? 0 : 180f,0));
        }

        #region wall n ground check
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * playerHalfHeight, groundCheckRadius, _ground);
        visual.radius = wallCheckRadius;
        isWalled = Physics.CheckSphere(_wallCheck.transform.position, wallCheckRadius, _ground);
        #endregion

        #region wall grab check
        isWallGrabQueued = isWalled && isWallGrabbedPressed && stamina.TryReduce(settings.wallGrabRateCost*Time.deltaTime);
        #endregion

        #region jump check
        if (isGrounded || isWallGrabQueued) {
            _coyoteTimer = coyoteTime;
            haveJump = true;
        }
        else if (isJumpQueued) {
            _coyoteTimer = 0; 
        }

        _coyoteTimer = Mathf.Max(_coyoteTimer - Time.deltaTime, 0);
        _jumpBufferTimer = Mathf.Max(_jumpBufferTimer - Time.deltaTime, 0);

        isCoyote = _coyoteTimer > 0;
        isJumpQueued = _jumpBufferTimer > 0 && haveJump && isCoyote;

        if (isJumpQueued) { 
            haveJump = false;
        };
        #endregion

        #region run check
        isRunQueued = isRunPressed && stamina.Check(1);
        #endregion

        #region dash check
        isDashQueued = _dashTimer <= 0 && isDashPressed;
        if (isDashQueued) {
            _dashTimer = settings.dashCooldown;
        }

        _dashTimer = Mathf.Max(_dashTimer - Time.deltaTime, 0);
        if (isDashQueued) Debug.Log(isDashQueued);
        #endregion
    }
    private void OnFrenzy() {
       if (!health.TryReduce(health.frenzyCost)) return;
        stamina.Frenzy();
    }
    #region input listeners
    private void OnJump(bool value) {
        isJumpPressed = value;
        if (value) { _jumpBufferTimer = settings.jumpBuffer; }
    }
    private void OnMove(int value) => dirHorizontal = value;
    private void OnDash(bool value){
        isDashPressed = value;
    }
    private void OnDashAim(bool value) => isDashAim = value;
    private void MousePosition(Vector2 value) => mousePos = value;
    private void OnRun(bool value) => isRunPressed = value;
    private void OnWallGrab(bool value) => isWallGrabbedPressed = value;
    private void OnWallSlide(int value) => dirVertical = value;
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
