using UnityEngine;

public class DashState : State {
    public DashState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override void Enter() {
        Debug.Log("dashing!");
        stateMachine.ChangeState(stateMachine.idleState);
    }
}
