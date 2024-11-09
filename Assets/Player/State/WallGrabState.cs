using UnityEngine;

public class WallGrabState : State
{
    public WallGrabState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override bool isUninterruptable { get; protected set; }
    public override void Enter() {
        isUninterruptable = true;
        playerController.rb.useGravity = false;
        playerController.rb.linearVelocity = Vector3.zero;
    }
    public override void Update() {
        if (!playerController.isWallGrabbedPressed || playerController.dirVertical != 0 || !playerController.isWalled || playerController.isJumpQueued) {
            isUninterruptable = false;
        }
    }
    public override void Exit() {
        playerController.rb.useGravity = true;
    }
}
