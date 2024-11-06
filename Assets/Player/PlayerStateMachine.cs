using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour {

    [SerializeField] private PlayerController playerController;

    private State currentState;

    public WalkState walkState;
    public IdleState idleState;
    public JumpState jumpState;
    public FallState fallState;
    public DashState dashState;
    private void Awake() {
        idleState = new IdleState(this, playerController);
        walkState = new WalkState(this, playerController);
        jumpState = new JumpState(this, playerController);
        fallState = new FallState(this, playerController);
        dashState = new DashState(this, playerController);

        currentState = idleState;
    }
    private void Start() {
        currentState?.Enter();
    }
    private void Update() {
        currentState?.Update();
    }
    private void FixedUpdate() {
        currentState?.FixedUpdate();
    }
    public void ChangeState(State newState) {
        currentState.Exit();

        currentState = newState;
        currentState.Enter();
        //Debug.Log(currentState.ToString());
    }
}
