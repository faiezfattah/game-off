using UnityEngine;

public class RunState : State {
    public RunState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override void Update() {
        if (playerController.dir == 0) {
            stateMachine.ChangeState(stateMachine.idleState);
        }
        if (playerController.isJumpPressed) {
            stateMachine.ChangeState(stateMachine.jumpState);
        }
        if (!playerController.isGrounded) {
            stateMachine.ChangeState(stateMachine.fallState);
        }
    }

    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dir * playerController.runSpeed, -playerController.runSpeed, playerController.runSpeed), 0, 0);
    }
}
