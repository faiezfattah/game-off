using UnityEngine;

public class WallSlideState : State {

    private       SfxParams _sfx;
    private const string    _id = "WALL SLIDE";
    public WallSlideState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
        _sfx = new SfxParams(playerController.playerAudio.wallSlide);
        _sfx.WithId(_id);
    }
    public override bool isUninterruptable { get; protected set; }
    public override void Enter() {
        isUninterruptable = true;
        playerController.rb.useGravity = false;
        playerController.animator.Play("Wall Slide Down");
    }
    public override void FixedUpdate() {
        playerController.playerAudio.Play(_sfx);
        
        float dir = Mathf.Clamp(playerController.settings.walkSpeed * playerController.dirVertical/2, -playerController.settings.walkSpeed, playerController.settings.walkSpeed);
        //playerController.rb.linearVelocity += new Vector3(0, dir, 0);
        playerController.rb.AddForce(new Vector3(0, dir, 0), ForceMode.Impulse);
        
        if (playerController.dirVertical > 0) {
            playerController.animator.Play("Wall Slide Up");
        } else playerController.animator.Play("Wall Slide Down");
        
        if (!playerController.stamina.TryReduce(playerController.settings.wallSlideRateCost * Time.deltaTime)  || !playerController.isWallGrabbedPressed  || !playerController.isJumpQueued || playerController.dirVertical == 0) {
            isUninterruptable = false;
        }
    }
    public override void Exit() {
        playerController.rb.useGravity = true;
    }
}
