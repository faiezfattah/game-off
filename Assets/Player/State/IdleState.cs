using UnityEngine;

public class IdleState : State
{
    public IdleState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) { }
    public override void Update() {

    }
}
