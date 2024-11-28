using UnityEngine;
using UnityEngine.TextCore.Text;

public class WalkState : State {
    
    private SfxParams _sfx;
    private const string _id = "WALK";
    public WalkState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine,
        playerController) {
        
        _sfx = new SfxParams(playerController.playerAudio.walkSfx)
            .WithId(_id);
    }
    public override void Enter() {
        playerController.playerAudio.Play(_sfx, true);
        playerController.animator.Play("Walk");
    }

    public override void FixedUpdate() {
        playerController.rb.linearVelocity += new Vector3(Mathf.Clamp(playerController.dirHorizontal * playerController.settings.walkSpeed, -playerController.settings.walkSpeed, playerController.settings.walkSpeed), 0, 0);
    }

    public override void Exit() {
        playerController.playerAudio.Stop(_id);
    }
}
