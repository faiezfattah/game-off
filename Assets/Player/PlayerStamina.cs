using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float regenRate = 5; // per seconds
    [SerializeField] private float regenStopTime = 3; // per seconds

    [SerializeField] private float currentStamina;


    [Header("Stamina Cost")] //'rate' = per seconds
    public float runRateCost = 10f;
    public float dashCost = 40f;
    public float jumpCost = 20f;
    public float wallGrabRateCost = 5f;
    public float wallSlideRateCost = 10f;
    public float airControlRateCost = 50f;

    private Coroutine _stopRegen;
    private float _stopRegenTimer;
    void Start(){
        currentStamina = maxStamina;
    }
    public bool TryReduce(float amount) {
        if (!Check(amount)) return false;

        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        _stopRegenTimer = regenStopTime;
        return true;
    }
    public void Increase(float amount) {
        currentStamina += amount;
        Mathf.Clamp(currentStamina, 0, maxStamina);
    }
    public bool Check(float amount) {
        return currentStamina >= amount;
    }
    private void Update() {
        _stopRegenTimer -= Time.deltaTime;
        if (currentStamina >= maxStamina || _stopRegenTimer > 0) return;

        Increase(regenRate * Time.deltaTime);
    }
}
