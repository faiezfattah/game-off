using System;
using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private PlayerMovementSettings settings;
    [SerializeField] private PlayerData _data;
    //[SerializeField] private float maxStamina = 100;
    //[SerializeField] private float regenRate = 5; // per seconds
    //[SerializeField] private float regenStopTime = 3; // per seconds\


    //[Header("Stamina Cost")] //'rate' = per seconds
    //public float runRateCost = 10f;
    //public float dashCost = 40f;
    //public float jumpCost = 20f;
    //public float wallGrabRateCost = 5f;
    //public float wallSlideRateCost = 10f;
    //public float airControlRateCost = 50f;

    private Coroutine _stopRegen;
    private float _stopRegenTimer;
    void Start(){
        _data.stamina = settings.maxStamina;
    }
    public bool TryReduce(float amount) {
        if (!Check(amount)) return false;

        _data.stamina -= amount;
        _data.stamina = Mathf.Clamp(_data.stamina, 0, settings.maxStamina);
        _stopRegenTimer = settings.regenStopTime;
        Debug.Log("decreased" + amount);
        return true;
    }
    public void Increase(float amount) {
        _data.stamina += amount;
        Mathf.Clamp(_data.stamina, 0, settings.maxStamina);
    }
    public bool Check(float amount) {
        return _data.stamina >= amount;
    }
    public void Frenzy() {
        _data.stamina = settings.maxStamina;
    }
    private void Update() {
        _stopRegenTimer -= Time.deltaTime;
        if (_data.stamina >= settings.maxStamina || _stopRegenTimer > 0) return;

        Increase(settings.regenRate * Time.deltaTime);
    }
}
