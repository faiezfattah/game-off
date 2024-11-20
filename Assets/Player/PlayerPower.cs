using System;
using UnityEngine;

public class PlayerPower : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    public void PowerUp(Type state, bool value) {
        _playerData.powerUp[state] = value;
    }
}
