using UnityEngine;

public class FallState : State {
    public FallState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(
            playerController.dir * playerController.walkSpeed, 0, 0);

        playerController.rb.AddForce(new Vector3(0, playerController.fallSpeed, 0), ForceMode.Acceleration);

        if (playerController.isGrounded) {
            playerController.isJumping = false;
            stateMachine.ChangeState(stateMachine.idleState);
        }
    } 
}
