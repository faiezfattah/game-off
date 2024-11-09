using UnityEngine;

public class JumpState : State {
    public JumpState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    private float _duration;
    public override bool isUninterruptable { get; protected set; }
    public override void Enter() {
        //if (playerController.isWallGrabQueued) {
        //    playerController.rb.AddForce(new Vector3(playerController.jumpForce * playerController.dirHorizontal, playerController.jumpForce, 0), ForceMode.Impulse);
        //}
        playerController.rb.AddForce(new Vector3(0, playerController.jumpForce, 0), ForceMode.Impulse);
        //Debug.Log("jumping");
        isUninterruptable = true;
    }
    public override void FixedUpdate() {
        _duration += Time.deltaTime;

        //mid air controls
        if (playerController.isRunPressed) {
            playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.runSpeed, -playerController.runSpeed, playerController.runSpeed), 0, 0);
        }
        else {
            playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.runSpeed, -playerController.walkSpeed, playerController.walkSpeed), 0, 0);
        }

        //break
        if (_duration >= playerController.jumpDuration || !playerController.isJumpPressed || playerController.isDashPressed) {
            isUninterruptable = false;
        }
    }
    public override void Exit() {
        _duration = 0f;
        playerController.rb.linearVelocity -= new Vector3(0, 0, 0);
    }
}
