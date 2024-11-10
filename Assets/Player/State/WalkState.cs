using UnityEngine;
using UnityEngine.TextCore.Text;

public class WalkState : State {
    public WalkState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) { }
    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.settings.walkSpeed, -playerController.settings.walkSpeed, playerController.settings.walkSpeed), 0, 0);
    }
}
