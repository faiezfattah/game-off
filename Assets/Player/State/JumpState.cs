using UnityEngine;

public class JumpState : State {
    public JumpState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    private float _duration;
    public override void Enter() {
        _duration = 0f;
        playerController.rb.AddForce(new Vector3(0, playerController.jumpForce, 0), ForceMode.Impulse);
    }
    public override void FixedUpdate() {
        _duration += Time.deltaTime;

        //mid air controls
        if (playerController.isRunPressed) {
            playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dir * playerController.runSpeed, -playerController.runSpeed, playerController.runSpeed), 0, 0);
        }
        else {
            playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dir * playerController.runSpeed, -playerController.walkSpeed, playerController.walkSpeed), 0, 0);
        }

        //fall
        if (_duration >= playerController.jumpDuration || !playerController.isJumpPressed) {
            stateMachine.ChangeState(stateMachine.fallState);
        }
    }
    public override void Exit() {
        playerController.isJumpQueue = false;
    }
}
