using UnityEngine;

public class FrenzyState : State {
    public FrenzyState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    public override void Enter() {
        playerController.stamina.Increase(playerController.settings.maxStamina);
    }
    public override void FixedUpdate() {
    }
    public override void Exit() {
        playerController.health.TryReduce(1);
    }
}
