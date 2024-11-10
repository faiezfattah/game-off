using UnityEngine;

public class RunState : State {
    public RunState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }

    public override void FixedUpdate() {
        if (!playerController.stamina.TryReduce(playerController.settings.runRateCost * Time.deltaTime)) return;
        playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.settings.runSpeed, -playerController.settings.runSpeed, playerController.settings.runSpeed), 0, 0);
    }
}
