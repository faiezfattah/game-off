using UnityEngine;

public class FallState : State {
    public FallState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(
            playerController.dir * playerController.aircControl, 0, 0);

        if (playerController.isRunPressed) {
            playerController.rb.AddForce(new Vector3(0, playerController.fallFastForce, 0), ForceMode.Acceleration);
        }
        else {
            playerController.rb.AddForce(new Vector3(0, playerController.fallForce, 0), ForceMode.Acceleration);
        }

        if (playerController.isGrounded || playerController.rb.linearVelocity.y < 0.1) {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    } 
}
