using UnityEngine;

public class RunState : State {
    public RunState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }

    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dir * playerController.runSpeed, -playerController.runSpeed, playerController.runSpeed), 0, 0);
    }
}
