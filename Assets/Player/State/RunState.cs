using UnityEngine;

public class RunState : State {
    
    private SfxParams sfx;
    private string    _id = "RUN";
    
    public RunState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
        sfx = new SfxParams(playerController.playerAudio.runSfx)
            .WithId(_id);
    }

    public override void Enter() {
        playerController.playerAudio.Play(sfx, true);
        playerController.animator.Play("Run");
    }

    public override void FixedUpdate() {
        if (!playerController.stamina.TryReduce(playerController.settings.runRateCost * Time.deltaTime)) return;
        playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.settings.runSpeed, -playerController.settings.runSpeed, playerController.settings.runSpeed), 0, 0);
    }

    public override void Exit() {
        playerController.playerAudio.Stop(_id);
    }
}
