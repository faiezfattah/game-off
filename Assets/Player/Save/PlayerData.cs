using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    //here exist all data
    //the no-save exist for ui
    //this data need to be rebuild.
    [Header("Save Data")]
    public int health = 3;

    [Space(10)]
    public Dictionary<Type, bool> powerUp = new();
    private void OnEnable() {
        //defaults
        powerUp.Add(typeof(IdleState), true);
        powerUp.Add(typeof(WalkState), true);
        powerUp.Add(typeof(FallState), true);

        //obtainable
        powerUp.Add(typeof(RunState), false);

        powerUp.Add(typeof(JumpState), false);

        powerUp.Add(typeof(DashState), false);

        powerUp.Add(typeof(WallGrabState), false);
        powerUp.Add(typeof(WallSlideState), true);

        powerUp.Add(typeof(FrenzyState), false);
    }

    [Header("No-save Data")]
    public float stamina;

    //todo: implement rebuild
}