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
    [SerializeField] private LayerMask _walkableLayer;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerStateMachine _statemMachine;

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
    public bool isDashCanceled;

    [Space(10)]
    public bool isGrounded;

    [Space(10)]
    public bool isWalled;
    public bool isWallGrabbedPressed;
    public bool isWallGrabQueued;

    [Space(10)]
    public bool isSafe;

    [Space(10)]
    [SerializeField] private float radius = 2f;

    private float playerHalfHeight = 0.5f;
    private Vector3 _linearVelocity;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private float _dashTimer;
    void Start(){
        playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!visual) visual = GetComponent<PlayerVisualizer>();
        if (_walkableLayer == 0) _walkableLayer = LayerMask.GetMask("Ground");
        _linearVelocity = rb.linearVelocity;
    }
    void Update() {
        if (dirHorizontal != 0) {
            _rotatingContainer.transform.rotation = Quaternion.Euler(new Vector3 (0, dirHorizontal > 0? 0 : 180f,0));
        }

        GroundWallCheck();
        UpdateSafePlace();
        WallGrabCheck();
        JumpCheck();
        RunCheck();
        DashCheck();
    }
    private void OnInteract() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders) {
            if (hitCollider.TryGetComponent<IInteractable>(out var interactable)) {
                interactable.Interact();
                return; // break the foreach loop on the first success
            }
        }
    }
        private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void GroundWallCheck() {
        //isGrounded = Physics.CheckSphere(transform.position + Vector3.down * playerHalfHeight, groundCheckRadius, _walkableLayer);

        isGrounded = Physics.CheckBox(
            transform.position + Vector3.down * playerHalfHeight,
            Vector3.one * groundCheckRadius,
            Quaternion.identity,
            _walkableLayer
        );

        visual.radius = wallCheckRadius;
        isWalled = Physics.CheckSphere(_wallCheck.transform.position, wallCheckRadius, _walkableLayer);
    }

    private void UpdateSafePlace() {
        Collider[] hit = Physics.OverlapBox(
            transform.position + Vector3.down * playerHalfHeight,
            Vector3.one * groundCheckRadius,
            Quaternion.identity,
            _walkableLayer
        );

        foreach (var hitCollider in hit) {
            isSafe = !hitCollider.CompareTag("Unsafe");
        }
        if (isGrounded && isSafe) {
            _playerData.lastSafePlace = transform.position;
        }
    }
    private void WallGrabCheck() {
        isWallGrabQueued = isWalled && isWallGrabbedPressed && stamina.Check(settings.wallSlideRateCost * Time.deltaTime);
    }
    private void JumpCheck() {
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
    }
    private void RunCheck() {
        isRunQueued = isRunPressed && stamina.Check(1);
    }
    private void DashCheck() {
        isDashQueued = _dashTimer <= 0 && isDashPressed && !isDashCanceled;
        if (isDashQueued) {
            _dashTimer = settings.dashCooldown;
        }

        _dashTimer = Mathf.Max(_dashTimer - Time.deltaTime, 0);
        //if (isDashQueued) Debug.Log(isDashQueued);
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Particle HIT " + collider.name);
    }
    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.cyan;

    //    Vector3 center = transform.position + Vector3.down * playerHalfHeight;
    //    Vector3 size = new Vector3(groundCheckRadius * 2, groundCheckRadius * 2, groundCheckRadius * 2);
    //    Gizmos.DrawWireCube(center, size);
    //}
    private void OnFrenzy() {
        //if (!_playerData.PowerUp[typeof(FrenzyState)]) return;
        if (!health.Check(health.frenzyCost)) return;
        _statemMachine.ChangeState(_statemMachine.frenzyState); 
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
    private void OnDashCancel(bool value) => isDashCanceled = value;
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
        _inputReader.DashCancelEvent += OnDashCancel;
        _inputReader.InteractEvent += OnInteract;
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
        _inputReader.DashCancelEvent -= OnDashCancel;
        _inputReader.InteractEvent -= OnInteract;
    }

    #endregion
}
