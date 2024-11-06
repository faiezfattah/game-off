using UnityEngine;

public class DashState : State {
    public DashState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    private float _duration;
    public override void Enter() {
        //playerController.rb.useGravity = false;
        playerController.rb.linearVelocity = playerController.rb.linearVelocity / 2;
    }
    public override void FixedUpdate() {
        _duration += Time.deltaTime;

        Vector3 dir = new Vector3(playerController.dashDir.x * playerController.dashForce, playerController.dashDir.y * playerController.dashForce, 0);
        playerController.rb.AddForce(dir, ForceMode.Force);

        if (_duration >= playerController.dashDuration) {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }
    public override void Exit() { 
        //playerController.rb.useGravity = true;
        playerController.rb.linearVelocity = new Vector3(0, 0, 0);
        _duration = 0f;
    }
}
