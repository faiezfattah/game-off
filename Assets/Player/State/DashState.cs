using System.Text;
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
        if(!playerController.stamina.Check(playerController.settings.dashCost)) return;
        isUninterruptable = true;
        _duration = 0f;
        _centerPos = playerController.mousePos;
        playerController.rb.linearVelocity = new Vector3(0, 0, 0);

    }
    public override void Update() {
        if (!playerController.isDashPressed) return;
        if (playerController.isDashCanceled) { isUninterruptable=false; }
        dir = playerController.mousePos - _centerPos;
        dir = dir.normalized;
        playerController.visual.DrawDashingLine(dir * 100);
    }
    public override void FixedUpdate() {
        if (playerController.isDashPressed) return;
        playerController.visual.DisableDashingLine();

        _duration += Time.deltaTime;

        Vector3 dash = new Vector3(dir.x * playerController.settings.dashForce, dir.y * playerController.settings.dashForce, 0);
        playerController.rb.AddForce(dash, ForceMode.Force);

        if (_duration > playerController.settings.dashDuration) {
            isUninterruptable = false;
        }
        else if (playerController.isJumpQueued || playerController.isWallGrabQueued) {
            isUninterruptable = false;
        }
    }
    public override void Exit() {
        //if (playerController.isJumpQueued) return;
        //if (!playerController.isDashCanceled && !playerController.stamina.TryReduce(playerController.settings.dashCost)) return;

        if (!playerController.isDashCanceled) playerController.stamina.TryReduce(playerController.settings.dashCost);
        playerController.visual.DisableDashingLine();
        playerController.rb.linearVelocity = playerController.rb.linearVelocity * playerController.settings.dashVelocityRetention;
    }
}
