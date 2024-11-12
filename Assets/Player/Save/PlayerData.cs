using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    //here exist all data.
    //two type: save and no-save
    [Header("Saveable Data")]
    public int health;
    
    [Header("No-save Data")]
    public float stamina;
}
