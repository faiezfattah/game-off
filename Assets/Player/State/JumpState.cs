using UnityEngine;

public class JumpState : State {
    
    private         float     _duration;
    public override bool      isUninterruptable { get; protected set; }
    private         SfxParams sfx;
    private         string    _id = "JUMP";
    public JumpState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
        sfx = new SfxParams(playerController.playerAudio.jumpSfx)
            .WithId(_id);
    }
    public override void Enter() {
        //if (playerController.isWallGrabQueued) {
        //    playerController.rb.AddForce(new Vector3(playerController.settings.jumpForce * playerController.dirHorizontal, playerController.settings.jumpForce, 0), ForceMode.Impulse);
        //}
        if (!playerController.stamina.TryReduce(playerController.settings.jumpCost)) return;
        playerController.rb.AddForce(new Vector3(0, playerController.settings.jumpForce, 0), ForceMode.Impulse);
        //Debug.Log("jumping");
        isUninterruptable = true;
        playerController.playerAudio.Play(sfx);
    }
    public override void FixedUpdate() {
        _duration += Time.deltaTime;

        //mid air controls
        if (playerController.isRunPressed) {
            playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.settings.runSpeed, -playerController.settings.runSpeed, playerController.settings.runSpeed), 0, 0);
        }
        else {
            playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.settings.walkSpeed, -playerController.settings.walkSpeed, playerController.settings.walkSpeed), 0, 0);
        }

        //break
        if (_duration >= playerController.settings.jumpDuration || !playerController.isJumpPressed || playerController.isDashQueued) {
            isUninterruptable = false;
        }
    }
    public override void Exit() {
        _duration = 0f;
        playerController.rb.linearVelocity -= new Vector3(0, 0, 0);
        playerController.playerAudio.Stop(_id);
    }
}
