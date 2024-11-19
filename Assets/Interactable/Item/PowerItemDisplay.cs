using System;
using UnityEngine;

public class PowerItemDisplay : MonoBehaviour, IInteractable {
    [SerializeField] private PlayerData _playerData;
    
    enum PowerType {
        Running,
        Jumping,
        Dashing,
        Walling,
        Frenzying
    }

    [SerializeField] private PowerType type;

    public void Interact() {
        PowerUp(ConvertPowerType(), true);
        if (ConvertPowerType() == typeof(WallGrabState)) PowerUp(typeof(WallSlideState), true);
    }
    private Type ConvertPowerType() {
        if (type == PowerType.Running) return typeof(RunState);
        else if (type == PowerType.Jumping) return typeof(JumpState);
        else if (type == PowerType.Dashing) return typeof(DashState);
        else if(type == PowerType.Walling) return typeof(WallGrabState);
        else if(type == PowerType.Frenzying) return typeof(FrenzyState);
        return null;
    }
    public void PowerUp(Type state, bool value) {
        _playerData.powerUp[state] = value;
    }
}
