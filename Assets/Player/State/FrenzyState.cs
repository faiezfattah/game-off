using UnityEngine;

public class FrenzyState : State {
    private SfxParams sfx;
    public FrenzyState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
        sfx = new SfxParams(playerController.playerAudio.frenzy);
    }
    public override void Enter() {
        playerController.stamina.Increase(playerController.settings.maxStamina);
        playerController.playerAudio.Play(sfx);
    }
    public override void FixedUpdate() {
    }
    public override void Exit() {
        playerController.health.TryReduce(1);
    }
}
