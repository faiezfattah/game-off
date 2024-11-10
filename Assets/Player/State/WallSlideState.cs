using UnityEngine;

public class WallSlideState : State {
    public WallSlideState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override bool isUninterruptable { get; protected set; }
    public override void Enter() {
        isUninterruptable = true;
        playerController.rb.useGravity = false;
    }
    public override void Update() {
        float dir = Mathf.Clamp(playerController.settings.walkSpeed * playerController.dirVertical, -playerController.settings.walkSpeed, playerController.settings.walkSpeed);
        //playerController.rb.linearVelocity += new Vector3(0, dir, 0);
        playerController.rb.AddForce(new Vector3(0, dir, 0), ForceMode.VelocityChange);

        if (playerController.dirVertical == 0 || !playerController.isWallGrabbedPressed || !playerController.isWalled || !playerController.isJumpQueued) {
            isUninterruptable = false;
        }
    }
    public override void Exit() {
        playerController.rb.useGravity = true;
    }
}
