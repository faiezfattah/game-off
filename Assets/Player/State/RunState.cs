using UnityEngine;

public class RunState : State {
    public RunState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }

    public override void FixedUpdate() {
        if (!playerController.stamina.TryReduce(playerController.stamina.runRateCost * Time.deltaTime)) return;
        playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.runSpeed, -playerController.runSpeed, playerController.runSpeed), 0, 0);
    }
}
