using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private PlayerMovementSettings settings;
    //[SerializeField] private float maxStamina = 100;
    //[SerializeField] private float regenRate = 5; // per seconds
    //[SerializeField] private float regenStopTime = 3; // per seconds

    [SerializeField] private float currentStamina;


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
        currentStamina = settings.maxStamina;
    }
    public bool TryReduce(float amount) {
        if (!Check(amount)) return false;

        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, settings.maxStamina);
        _stopRegenTimer = settings.regenStopTime;
        return true;
    }
    public void Increase(float amount) {
        currentStamina += amount;
        Mathf.Clamp(currentStamina, 0, settings.maxStamina);
    }
    public bool Check(float amount) {
        return currentStamina >= amount;
    }
    public void Frenzy() {
        currentStamina = settings.maxStamina;
    }
    private void Update() {
        _stopRegenTimer -= Time.deltaTime;
        if (currentStamina >= settings.maxStamina || _stopRegenTimer > 0) return;

        Increase(settings.regenRate * Time.deltaTime);
    }
}
