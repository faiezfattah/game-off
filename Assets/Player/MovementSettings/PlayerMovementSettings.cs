using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Player/MovementSettings")]
public class PlayerMovementSettings : ScriptableObject
{
    [Header("Movement")]
    public float walkSpeed = 1f;
    public float runSpeed = 2.5f;

    [Space(10)]
    public float dashForce = 500;
    public float dashDuration = 0.3f;
    public float dashCooldown = 1f;
    public float dashVelocityRetention = 0.75f;

    [Space(10)]
    public float jumpForce = 50f;
    public float jumpDuration = 0.5f;
    public float jumpBuffer = 0.1f;

    [Space(10)]
    public float fallForce = -150f;
    public float fallSlowForce = -50f;
    public float airControl = 1.5f;

    [Header("Stamina Settings")]
    public float maxStamina = 100;
    public float regenRate = 5; // per seconds
    public float regenStopTime = 3; // per seconds

    [Header("Stamina Cost")] //'rate' = per seconds
    public float runRateCost = 10f;
    public float dashCost = 40f;
    public float jumpCost = 20f;
    public float wallGrabRateCost = 5f;
    public float wallSlideRateCost = 10f;
    public float airControlRateCost = 50f;

}
