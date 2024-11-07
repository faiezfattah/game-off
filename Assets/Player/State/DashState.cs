using UnityEngine;

public class DashState : State {
    public DashState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    private float _duration;
    public override bool isUninterruptable { get; protected set; }
    public override void Enter() {
        //playerController.rb.useGravity = false;
        isUninterruptable = true;
        playerController.rb.linearVelocity = playerController.rb.linearVelocity / 2;
        Debug.Log("dashing!");
    }
    public override void FixedUpdate() {
        _duration += Time.deltaTime;

        Vector3 dir = new Vector3(playerController.dashDir.x * playerController.dashForce, playerController.dashDir.y * playerController.dashForce, 0);
        playerController.rb.AddForce(dir, ForceMode.Force);

        if (_duration > playerController.dashDuration) {
            isUninterruptable = false;
        }
    }
    public override void Exit() { 
        //playerController.rb.useGravity = true;
        playerController.rb.linearVelocity = new Vector3(0, 0, 0);
        _duration = 0f;
    }
}
