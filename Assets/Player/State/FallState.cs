using UnityEngine;

public class FallState : State {
    private          SfxParams _sfxFall;
    private readonly string    _id = "FALL";
    private readonly SfxParams _sfxLand;
    private readonly string    _id2 = "LAND";
    public FallState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
        _sfxLand = new SfxParams(playerController.playerAudio.land);
        _sfxLand.WithId(_id2);
        //_sfxFall = new SfxParams(playerController.playerAudio.
    }

    public override void Enter() {
        playerController.animator.Play("Fall");
        //playerController.playerAudio.Play(_sfxFall);
    }

    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(
            playerController.dirHorizontal * playerController.settings.airControl, 0, 0);

        if (playerController.isJumpPressed && playerController.stamina.TryReduce(playerController.settings.airControlRateCost * Time.deltaTime)) {
            playerController.rb.AddForce(new Vector3(0, playerController.settings.fallSlowForce, 0), ForceMode.Acceleration);
            return;
        }

        playerController.rb.AddForce(new Vector3(0, playerController.settings.fallForce, 0), ForceMode.Acceleration);
    }

    public override void Exit() {
        playerController.playerAudio.Play(_sfxLand);
    }
}
