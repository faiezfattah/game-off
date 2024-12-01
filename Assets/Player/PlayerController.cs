using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour {
    [FormerlySerializedAs("_inputReader")]
    [Header("Private Reference")]
    [SerializeField]
    private InputReader inputReader;

    [FormerlySerializedAs("_rotatingContainer")]
    [SerializeField]
    private GameObject rotatingContainer;

    [FormerlySerializedAs("_wallCheck")]
    [SerializeField]
    private GameObject wallCheck;

    [FormerlySerializedAs("_walkableLayer")]
    [SerializeField]
    private LayerMask walkableLayer;

    [FormerlySerializedAs("_playerData")]
    [SerializeField]
    private PlayerData playerData;

    [FormerlySerializedAs("_statemMachine")]
    [SerializeField]
    private PlayerStateMachine statemMachine;

    [Header("Public Reference")]
    public PlayerVisualizer visual;

    public Rigidbody              rb;
    public PlayerStamina          stamina;
    public PlayerHealth           health;
    public PlayerMovementSettings settings;
    public PlayerAudio            playerAudio;
    public Animator               animator;

    [Header("Movement QoL")]
    public float coyoteTime = 0.5f;

    public float groundCheckRadius = 0.1f;
    public float wallCheckRadius   = 1f;

    [Space(10)]
    public string currentState;

    [Header("Inputs and derived states")]
    public float dirHorizontal;

    public float   dirVertical;
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
    [SerializeField]
    private float radius = 2f;

    private float     _playerHalfHeight = 0.5f;
    private Vector3   _linearVelocity;
    private float     _coyoteTimer;
    private float     _jumpBufferTimer;
    private float     _dashTimer;
    private SpriteRenderer _sprite;

    private void Start() {
        _playerHalfHeight = GetComponent<CapsuleCollider>().height / 2;
        if (walkableLayer == 0) walkableLayer = LayerMask.GetMask("Ground");
        if (!rb) rb                           = GetComponent<Rigidbody>();
        if (!visual) visual                   = GetComponent<PlayerVisualizer>();
        if (!animator) animator               = GetComponent<Animator>();
        if (!_sprite) _sprite           = GetComponent<SpriteRenderer>();
        _linearVelocity = rb.linearVelocity;
    }

    private void Update() {
        if (dirHorizontal != 0) {
            rotatingContainer.transform.rotation = Quaternion.Euler(new Vector3(0, dirHorizontal > 0 ? 0 : 180f, 0));
            _sprite.flipX = dirHorizontal > 0;
        }


        GroundWallCheck();
        UpdateSafePlace();
        WallGrabCheck();
        JumpCheck();
        RunCheck();
        DashCheck();
    }

    private void OnInteract() {
        // Debug.Log("interact pressed ");
        // var hitColliders = new Collider[] { };
        // var size         = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders);
        var hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders) {
            if (!hitCollider.TryGetComponent<IInteractable>(out var interactable)) continue;
            interactable.Interact();
            return; // break the foreach loop on the first success
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void GroundWallCheck() {
        //isGrounded = Physics.CheckSphere(transform.position + Vector3.down * playerHalfHeight, groundCheckRadius, _walkableLayer);

        isGrounded = Physics.CheckBox(
            transform.position + Vector3.down * _playerHalfHeight,
            Vector3.one * groundCheckRadius,
            Quaternion.identity,
            walkableLayer
        );

        visual.radius = wallCheckRadius;
        isWalled      = Physics.CheckSphere(wallCheck.transform.position, wallCheckRadius, walkableLayer);
    }

    private void UpdateSafePlace() {
        var hit = new Collider[] { };
        var size = Physics.OverlapBoxNonAlloc(transform.position + Vector3.down * _playerHalfHeight,
            Vector3.one * groundCheckRadius, hit, Quaternion.identity, walkableLayer);

        foreach (var hitCollider in hit) {
            isSafe = !hitCollider.CompareTag("Unsafe");
        }

        if (isGrounded && isSafe) {
            playerData.lastSafePlace = transform.position;
        }
    }

    private void WallGrabCheck() {
        isWallGrabQueued = isWalled && isWallGrabbedPressed &&
                           stamina.Check(settings.wallSlideRateCost * Time.deltaTime);
    }

    private void JumpCheck() {
        if (isGrounded || isWallGrabQueued) {
            _coyoteTimer = coyoteTime;
            haveJump     = true;
        }
        else if (isJumpQueued) {
            _coyoteTimer = 0;
        }

        _coyoteTimer     = Mathf.Max(_coyoteTimer - Time.deltaTime, 0);
        _jumpBufferTimer = Mathf.Max(_jumpBufferTimer - Time.deltaTime, 0);

        isCoyote     = _coyoteTimer > 0;
        isJumpQueued = _jumpBufferTimer > 0 && haveJump && isCoyote;

        if (isJumpQueued) {
            haveJump = false;
        }

        ;
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

    void OnTriggerEnter(Collider contextCollider) {
        //Debug.Log("Particle HIT " + contextCollider.name);
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
        statemMachine.ChangeState(statemMachine.frenzyState);
    }

    #region input listeners

    private void OnJump(bool value) {
        isJumpPressed = value;
        if (value) {
            _jumpBufferTimer = settings.jumpBuffer;
        }
    }

    private void OnMove(int value) => dirHorizontal = value;

    private void OnDash(bool value) {
        isDashPressed = value;
    }

    private void OnDashAim(bool        value) => isDashAim = value;
    private void MousePosition(Vector2 value) => mousePos = value;
    private void OnRun(bool            value) => isRunPressed = value;
    private void OnWallGrab(bool       value) => isWallGrabbedPressed = value;
    private void OnWallSlide(int       value) => dirVertical = value;
    private void OnDashCancel(bool     value) => isDashCanceled = value;

    private void OnEnable() {
        inputReader.MoveEvent          += OnMove;
        inputReader.JumpEvent          += OnJump;
        inputReader.DashStartEvent     += OnDash;
        inputReader.RunEvent           += OnRun;
        inputReader.MousePositionEvent += MousePosition;
        inputReader.DashAimEvent       += OnDashAim;
        inputReader.WallGrabEvent      += OnWallGrab;
        inputReader.SlideEvent         += OnWallSlide;
        inputReader.FrenzyEvent        += OnFrenzy;
        inputReader.DashCancelEvent    += OnDashCancel;
        inputReader.InteractEvent      += OnInteract;
    }

    private void OnDisable() {
        inputReader.MoveEvent          -= OnMove;
        inputReader.JumpEvent          -= OnJump;
        inputReader.DashStartEvent     -= OnDash;
        inputReader.RunEvent           -= OnRun;
        inputReader.MousePositionEvent -= MousePosition;
        inputReader.DashAimEvent       -= OnDashAim;
        inputReader.WallGrabEvent      -= OnWallGrab;
        inputReader.SlideEvent         -= OnWallSlide;
        inputReader.FrenzyEvent        -= OnFrenzy;
        inputReader.DashCancelEvent    -= OnDashCancel;
        inputReader.InteractEvent      -= OnInteract;
    }

    #endregion
}