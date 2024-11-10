using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    public int frenzyCost = 1;

    private float _currentHealth;
    void Start()
    {
        _currentHealth = maxHealth;
    }

    public bool TryReduce(int amount) {
        if (!Check(amount)) return false;

        _currentHealth -= amount;
        if (_currentHealth <= 0) Die();
        return true;
    }
    public bool Check(int amount) {
        return _currentHealth >= amount;
    }
    private void Die() {
        Debug.Log("die");
    }
}
