using UnityEngine;

public class FallState : State {
    public FallState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(
            playerController.dirHorizontal * playerController.settings.airControl, 0, 0);

        if (playerController.isJumpPressed && playerController.stamina.TryReduce(playerController.settings.airControlRateCost * Time.deltaTime)) {
            playerController.rb.AddForce(new Vector3(0, playerController.settings.fallSlowForce, 0), ForceMode.Acceleration);
            return;
        }

        playerController.rb.AddForce(new Vector3(0, playerController.settings.fallForce, 0), ForceMode.Acceleration);
    }
}
