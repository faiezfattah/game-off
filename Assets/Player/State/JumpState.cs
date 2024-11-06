using UnityEngine;

public class JumpState : State {
    public JumpState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }

    public override void Enter() {
        if (playerController.isJumping) return;

        playerController.rb.linearVelocity += (new Vector3(0, playerController.jumpForce, 0));
        Debug.Log("jumping");

        playerController.isJumping = true;
        stateMachine.ChangeState(stateMachine.fallState);
    }
}
