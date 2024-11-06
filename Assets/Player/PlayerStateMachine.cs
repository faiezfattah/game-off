using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour {

    [SerializeField] private PlayerController playerController;

    private State prevState;
    private State currentState;

    public WalkState walkState;
    public IdleState idleState;
    public JumpState jumpState;
    public FallState fallState;
    public DashState dashState;
    public RunState runState;
    private void Awake() {
        idleState = new IdleState(this, playerController);
        walkState = new WalkState(this, playerController);
        jumpState = new JumpState(this, playerController);
        fallState = new FallState(this, playerController);
        runState = new RunState(this, playerController);

        //global state
        dashState = new DashState(this, playerController);

        currentState = idleState;
    }
    private void Start() {
        currentState?.Enter();
    }
    private void Update() {
        currentState?.Update();

        //forcing global transition
        if (playerController.isDashPressed) {
            ChangeState(dashState);
        }
    }
    private void FixedUpdate() {
        currentState?.FixedUpdate();
    }
    public void ChangeState(State newState) {
        if (currentState == newState) return;

        currentState.Exit();
        prevState = currentState;

        currentState = newState;
        currentState.Enter();
        Debug.Log(currentState.ToString());
    }
}
