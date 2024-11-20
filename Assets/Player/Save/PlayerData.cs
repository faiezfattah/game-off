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
    public bool godMode = false;


    private void OnEnable() {

        #if UNITY_EDITOR
            godMode = true;
        #endif

        if (powerUp.Count == 0) {

            //defaults
            powerUp[typeof(IdleState)] = true;
            powerUp[typeof(WalkState)] = true;
            powerUp[typeof(FallState)] = true;

            //obtainable
            powerUp[typeof(RunState)] = godMode;
            powerUp[typeof(JumpState)] = godMode;
            powerUp[typeof(DashState)] = godMode;
            powerUp[typeof(WallGrabState)] = godMode;
            powerUp[typeof(WallSlideState)] = godMode;
            powerUp[typeof(FrenzyState)] = godMode;
        }
    }

    [Header("No-save Data")]
    public float stamina;

    //todo: implement rebuild
}