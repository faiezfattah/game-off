using Unity.VisualScripting;
using UnityEngine;

public class DashState : State {
    public DashState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
    }
    private float _duration;
    private Vector2 _centerPos;
    public Vector2 dir;
    public override bool isUninterruptable { get; protected set; }
    public override void Enter() {
        isUninterruptable = true;
        _duration = 0f;
        //playerController.rb.useGravity = false;

        _centerPos = playerController.mousePos;

        //playerController.rb.linearVelocity = playerController.rb.linearVelocity / 2;
        playerController.rb.linearVelocity = new Vector3(0, 0, 0);
    }
    public override void Update() {
        if (!playerController.isDashPressed) return;
        dir = playerController.mousePos - _centerPos;
        dir = dir.normalized;
        playerController.visual.DrawDashingLine(dir * 100);
    }
    public override void FixedUpdate() {
        if (playerController.isDashPressed) return;

        _duration += Time.deltaTime;

        Vector3 dash = new Vector3(dir.x * playerController.settings.dashForce, dir.y * playerController.settings.dashForce, 0);
        playerController.rb.AddForce(dash, ForceMode.Force);

        if (_duration > playerController.settings.dashDuration) {
            isUninterruptable = false;
        }
        else if (playerController.isJumpQueued) {
            isUninterruptable = false;
        }
    }
    public override void Exit() {
        //playerController.rb.useGravity = true;
        playerController.visual.DisableDashingLine();
        if (playerController.isJumpQueued) return;
        playerController.rb.linearVelocity = playerController.rb.linearVelocity * playerController.settings.dashVelocityRetention;
        //new Vector3(playerController.rb.linearVelocity.x * playerController.dashVelocityRetention, playerController.rb.linearVelocity.y * playerController.dashVelocityRetention, 0);
    }
}
