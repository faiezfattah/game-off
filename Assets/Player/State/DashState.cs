using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class DashState : State {
    private float     _duration;
    private Vector2   _centerPos;
    public  Vector2   dir;
    private SfxParams sfxEnter;
    private SfxParams sfxExit;
    private string    _enterId = "DASH_ENTER";
    private string    _exitId = "DASH_EXIT";
    public DashState(PlayerStateMachine stateMachine, PlayerController playerController) : base(stateMachine, playerController) {
        sfxEnter = new SfxParams(playerController.playerAudio.dashIn).WithId(_enterId);
        sfxExit = new SfxParams(playerController.playerAudio.dashOut).WithId(_exitId);
    }
    public override bool isUninterruptable { get; protected set; }
    public override void Enter() {
        if(!playerController.stamina.Check(playerController.settings.dashCost)) return;
        isUninterruptable = true;
        _duration = 0f;
        _centerPos = playerController.mousePos;
        playerController.rb.linearVelocity = new Vector3(0, 0, 0);
        playerController.playerAudio.Play(sfxEnter);
        Time.timeScale      = 0.3f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
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

        playerController.playerAudio.Play(sfxExit);
        
        if (_duration > playerController.settings.dashDuration) {
            isUninterruptable = false;
        }
        else if (playerController.isJumpQueued || playerController.isWallGrabQueued) {
            isUninterruptable = false;
        }
        Time.timeScale      = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    public override void Exit() {
        //if (playerController.isJumpQueued) return;
        //if (!playerController.isDashCanceled && !playerController.stamina.TryReduce(playerController.settings.dashCost)) return;

        if (!playerController.isDashCanceled) playerController.stamina.TryReduce(playerController.settings.dashCost);
        playerController.visual.DisableDashingLine();
        playerController.rb.linearVelocity = playerController.rb.linearVelocity * playerController.settings.dashVelocityRetention;
    }
}
