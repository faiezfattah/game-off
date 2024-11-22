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
    public Vector3 checkPoint;

    /// <summary>
    /// Always use try add on this array.
    /// </summary>

    public List<Type> Powers { private set; get; } = new();

    [Header("No-save Data")]
    public float stamina;
    public Vector3 lastSafePlace;

    //Function and utilites
    private void OnEnable() {
        Powers.Clear();
    }
    public bool TryAddPower(PowerItem power) {
        if (Powers.Count >= 3) return false;
        if (Powers.Contains(power.GetPowerType())) return false;
        Powers.Add(power.GetPowerType());
        Debug.Log("added: " + power.ToString());

        return true;
    }
    public void ReplacePower(PowerItem newPower, PowerItem oldPower) {
        Powers.Remove(oldPower.GetPowerType());
        Powers.Add(newPower.GetPowerType());
    }
    //todo: implement rebuild
}