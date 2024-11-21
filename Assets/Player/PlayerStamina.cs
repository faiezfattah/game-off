using System;
using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private PlayerMovementSettings settings;
    [SerializeField] private PlayerData _data;

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
        // Debug.Log("decreased" + amount);
        return true;
    }
    public void Increase(float amount) {
        _data.stamina += amount;
        Mathf.Clamp(_data.stamina, 0, settings.maxStamina);
    }
    public bool Check(float amount) {
        return _data.stamina >= amount;
    }
    private void Update() {
        _stopRegenTimer -= Time.deltaTime;
        if (_data.stamina >= settings.maxStamina || _stopRegenTimer > 0) return;

        Increase(settings.regenRate * Time.deltaTime);
    }
}
