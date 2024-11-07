using System.Collections.Generic;
using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour {
    public PlayerStateMachine(PlayerController playerController) { _playerController = playerController; }

    [SerializeField] private PlayerController _playerController;
    private Dictionary<Type, Dictionary<Type, Func<bool>>> transitionMatrix;

    private State prevState;
    private State currentState;

    public WalkState walkState;
    public IdleState idleState;
    public JumpState jumpState;
    public FallState fallState;
    public DashState dashState;
    public RunState runState;
    private void Awake() {
        transitionMatrix = new Dictionary<Type, Dictionary<Type, Func<bool>>>();

        idleState = new IdleState(this, _playerController);
        walkState = new WalkState(this, _playerController);
        jumpState = new JumpState(this, _playerController);
        fallState = new FallState(this, _playerController);
        runState = new RunState(this, _playerController);
        dashState = new DashState(this, _playerController);

        //AddTransition(typeof(IdleState), typeof(WalkState), () => _playerController.dir != 0);
        //AddTransition(typeof(WalkState), typeof(IdleState), () => _playerController.dir == 0);
        //AddTransition(typeof(WalkState), typeof(RunState), () => _playerController.isRunPressed);
        //AddTransition(typeof(IdleState), typeof(JumpState), () => _playerController.isJumpPressed);
        //AddTransition(typeof(WalkState), typeof(JumpState), () => _playerController.isJumpPressed);
        //AddTransition(typeof(IdleState), typeof(DashState), () => _playerController.isDashPressed);
        //AddTransition(typeof(WalkState), typeof(DashState), () => _playerController.isDashPressed);
        //AddTransition(typeof(RunState), typeof(JumpState), () => _playerController.isJumpPressed);
        //AddTransition(typeof(RunState), typeof(DashState), () => _playerController.isDashPressed);
        //AddTransition(typeof(JumpState), typeof(FallState), () => !_playerController.isGrounded);
        //AddTransition(typeof(DashState), typeof(FallState), () => !_playerController.isGrounded);
        //AddTransition(typeof(WalkState), typeof(FallState), () => !_playerController.isGrounded);
        //AddTransition(typeof(RunState), typeof(FallState), () => !_playerController.isGrounded);
        //AddTransition(typeof(FallState), typeof(IdleState), () => _playerController.isGrounded);
        //AddTransition(typeof(FallState), typeof(WalkState), () => _playerController.dir != 0 && _playerController.isGrounded);

        // i hate this ngl.

        //to idle
        AddTransition(typeof(WalkState), typeof(IdleState), () => _playerController.isGrounded);
        //AddTransition(typeof(RunState), typeof(IdleState), () => _playerController.isGrounded);
        //AddTransition(typeof(JumpState), typeof(IdleState), () => _playerController.isGrounded);
        AddTransition(typeof(DashState), typeof(IdleState), () => _playerController.isGrounded);
        AddTransition(typeof(FallState), typeof(IdleState), () => _playerController.isGrounded);

        //to walk
        AddTransition(typeof(IdleState), typeof(WalkState), () => _playerController.dir != 0);

        //to run
        AddTransition(typeof(WalkState), typeof(RunState), () => _playerController.isRunPressed);


        //to jump
        AddTransition(typeof(IdleState), typeof(JumpState), () => _playerController.isJumpQueue && (_playerController.isCoyote || _playerController.isGrounded));
        AddTransition(typeof(WalkState), typeof(JumpState), () => _playerController.isJumpQueue && (_playerController.isCoyote || _playerController.isGrounded));
        AddTransition(typeof(RunState), typeof(JumpState), () => _playerController.isJumpQueue && (_playerController.isCoyote || _playerController.isGrounded));
        AddTransition(typeof(FallState), typeof(JumpState), () => _playerController.isJumpQueue && (_playerController.isCoyote || _playerController.isGrounded));


        //to dash
        AddTransition(typeof(IdleState), typeof(DashState), () => _playerController.isDashPressed);
        AddTransition(typeof(WalkState), typeof(DashState), () => _playerController.isDashPressed);
        AddTransition(typeof(RunState), typeof(DashState), () => _playerController.isDashPressed);
        AddTransition(typeof(JumpState), typeof(DashState), () => _playerController.isDashPressed);
        AddTransition(typeof(FallState), typeof(DashState), () => _playerController.isDashPressed);

        //to fall
        AddTransition(typeof(IdleState), typeof(FallState), () => !_playerController.isGrounded);
        AddTransition(typeof(WalkState), typeof(FallState), () => !_playerController.isGrounded);
        AddTransition(typeof(RunState), typeof(FallState), () => !_playerController.isGrounded);
        AddTransition(typeof(JumpState), typeof(FallState), () => !_playerController.isGrounded);
        AddTransition(typeof(DashState), typeof(FallState), () => !_playerController.isGrounded);


        currentState = idleState;
    }
    private void Start() {
        currentState?.Enter();
    }

    private void Update() {
        currentState?.Update();

        if (CanTransitionTo(typeof(WalkState))) {
            ChangeState(walkState);
        }
        else if (CanTransitionTo(typeof(FallState))) {
            ChangeState(fallState);
        }
        else if (CanTransitionTo(typeof(JumpState))) {
            ChangeState(jumpState);
        }
        else if (CanTransitionTo(typeof(RunState))) {
            ChangeState(runState);
        }
        else if (CanTransitionTo(typeof(IdleState))) {
            ChangeState(idleState);
        }
        else if (CanTransitionTo(typeof(DashState))) {
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
        Debug.Log(currentState.ToString() + ", previously " + prevState.ToString());
    }

    public bool CanTransitionTo(Type to) {
        if (transitionMatrix.TryGetValue(currentState.GetType(), out var transitions)) {
            foreach (var (targetState, condition) in transitions) {
                if (targetState == to && condition() && !currentState.isUninterruptable) {
                    return true;
                }
            }
        }
        return false;
    }

    private void AddTransition(Type from, Type to, Func<bool> condition) {
        if (!transitionMatrix.TryGetValue(from, out var transitions)) {
            transitions = new Dictionary<Type, Func<bool>>();
            transitionMatrix[from] = transitions;
        }
        transitions[to] = condition;
    }

}
