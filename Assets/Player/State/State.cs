using UnityEngine;

public abstract class State
{
    protected PlayerStateMachine stateMachine;
    protected PlayerController playerController;

    /// <summary>
    /// For state that need to execute until completion.
    /// </summary>
    public virtual bool isUninterruptable { get; protected set; } = false;
    public State(PlayerStateMachine stateMachine, PlayerController playerController) {
        this.stateMachine = stateMachine;
        this.playerController = playerController;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
