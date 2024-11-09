using UnityEngine;

public class FallState : State {
    public FallState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(
            playerController.dirHorizontal * playerController.aircControl, 0, 0);

        if (playerController.isJumpPressed) {
            playerController.rb.AddForce(new Vector3(0, playerController.fallSlowForce, 0), ForceMode.Acceleration);
        }
        else {
            playerController.rb.AddForce(new Vector3(0, playerController.fallForce, 0), ForceMode.Acceleration);
        }

    } 
}
