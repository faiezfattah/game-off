using UnityEngine;

public class IdleState : State
{
    public IdleState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) { }
    public override void Update() {
        // check if walking
        if (playerController.dir != 0) {
            stateMachine.ChangeState(stateMachine.walkState);
        }
        if (!playerController.isGrounded) {
            stateMachine.ChangeState(stateMachine.fallState);
        }
        if (playerController.isJumpQueue && (playerController.isGrounded || playerController.isCoyote)) {
            stateMachine.ChangeState(stateMachine.jumpState);
        }
        if (playerController.isRunPressed) {
            stateMachine.ChangeState(stateMachine.runState);
        }
    }
}
