using Unity.Properties;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerData _data;
    [SerializeField] private int maxHealth = 3;

    public int frenzyCost = 1;

    void Start()
    {
        _data.health = maxHealth;
    }

    public bool TryReduce(int amount) {
        if (!Check(amount)) return false;

        _data.health -= amount;
        if (_data.health <= 0) Die();
        return true;
    }
    public bool Check(int amount) {
        return _data.health >= amount;
    }
    private void Die() {
        Debug.Log("die");
    }
}
