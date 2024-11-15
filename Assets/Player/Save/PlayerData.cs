using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    //here exist all data
    //the no-save exist for ui
    [Header("Saveable Data")]
    public int health = 3;
    //public bool isRunEnabled = false;
    //public bool isJumpEnabled = false;
    //public bool isAirControllEnabled = false;
    //public bool isDashEnabled = false;
    //public bool isSlowDescentEnabled = false;
    //public bool isWallGrabEnable = false;

    [Header("No-save Data")]
    public float stamina;
}
