using System.Collections.Generic;
using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour {
    public PlayerStateMachine(PlayerController playerController) { _playerController = playerController; }

    [SerializeField] private PlayerController _playerController;
    private Dictionary<Type, Dictionary<Type, Func<bool>>> transitionMatrix;

    private State _prevState;
    private State _currentState;

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

        //to dash
        AddTransition(typeof(State), typeof(DashState), () => _playerController.isDashQueued);

        //to fall
        AddTransition(typeof(State), typeof(FallState), () => !_playerController.isGrounded);

        //to idle
        AddTransition(typeof(State), typeof(IdleState), () => _playerController.isGrounded);

        //to walk
        AddTransition(typeof(IdleState), typeof(WalkState), () => _playerController.dir != 0);

        //to run
        AddTransition(typeof(WalkState), typeof(RunState), () => _playerController.isRunPressed);

        //to jump
        AddTransition(typeof(IdleState), typeof(JumpState), () => _playerController.isJumpQueued);
        AddTransition(typeof(WalkState), typeof(JumpState), () => _playerController.isJumpQueued);
        AddTransition(typeof(RunState), typeof(JumpState), () => _playerController.isJumpQueued);
        AddTransition(typeof(FallState), typeof(JumpState), () => _playerController.isJumpQueued);

        _currentState = idleState;
    }
    private void Start() {
        _currentState?.Enter();
    }
    private void Update() {
        _currentState?.Update();
        // this has priority, top first. bottom last.
        // i hate it but idk what else to do


        if (CanTransitionTo(typeof(DashState))) {
            ChangeState(dashState);
        }
        else if (CanTransitionTo(typeof(JumpState))) {
            ChangeState(jumpState);
        }
        else if (CanTransitionTo(typeof(FallState))) {
            ChangeState(fallState);
        }
        else if (CanTransitionTo(typeof(RunState))) {
            ChangeState(runState);
        }
        else if (CanTransitionTo(typeof(WalkState))) {
            ChangeState(walkState);
        }
        else if (CanTransitionTo(typeof(IdleState))) {
            ChangeState(idleState);
        }
    }
    private void FixedUpdate() {
        _currentState?.FixedUpdate();
    }
    public void ChangeState(State newState) {
        if (_currentState == newState) return;

        _currentState.Exit();
        _prevState = _currentState;

        _currentState = newState;
        _currentState.Enter();
        Debug.Log(_currentState.ToString() + ", previously " + _prevState.ToString());
        _playerController.currentState = _currentState.ToString();
    }
    public bool CanTransitionTo(Type to) {
        if (transitionMatrix.TryGetValue(_currentState.GetType(), out var transitions)) {
            foreach (var (targetState, condition) in transitions) {
                if (targetState == to && condition() && !_currentState.isUninterruptable) {
                    return true;
                }
            }
        }
        if (transitionMatrix.TryGetValue(typeof(State), out transitions)) {
            foreach (var (targetState, condition) in transitions) {
                if (targetState == to && condition() && !_currentState.isUninterruptable) {
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
