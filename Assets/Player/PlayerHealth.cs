using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerData _data;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float zeroHealthTimer = 3;

    public int frenzyCost = 1;

    private Coroutine zeroHealth;
    void Start()
    {
        _data.health = maxHealth;
    }

    public bool TryReduce(int amount) {
        if (!Check(amount)) return false;

        _data.health -= amount;
        TeleportToSafe();

        if (_data.health <= 0) ZeroHealth();
        return true;
    }
    public bool Check(int amount) {
        return _data.health >= amount;
    }
    public void TeleportToSafe() {
        Vector3 loc = _data.lastSafePlace;

        if (loc == null) loc = _data.checkPoint;
        if (_data.checkPoint == null) { Debug.Log("Checkpoint and Last Safe Place missing"); return; }

        gameObject.transform.position = loc;
    }
    private void ZeroHealth() {
        if (zeroHealth == null) return;

        zeroHealth = StartCoroutine(ZeroHealthCoroutine());
    }
    private IEnumerator ZeroHealthCoroutine() {
        float timer = 0;
        while (timer < zeroHealthTimer) {
            timer += Time.deltaTime;
            ZeroHealthCheck();
            yield return null;
        }
        zeroHealth = null;
    }
    private void ZeroHealthCheck() {
        if (_data.health > 0) {
            StopCoroutine(zeroHealth);
        }
    }
    private void Die() {
        Debug.Log("died");
    }
}
