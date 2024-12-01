using System;
using UnityEngine;

public class PowerItem : MonoBehaviour, IInteractable {
    [SerializeField] private PlayerData _playerData;
    public enum PowerType {
        Running,
        Jumping,
        Dashing,
        Walling,
        Frenzying
    }

    public PowerType type;

    private MeshRenderer _mesh;
    private Collider _collider;
    private void Start() {
        _mesh = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        if (_playerData.Powers.Contains(GetPowerType(type))) gameObject.SetActive(false);
    }
    public void Interact() {
        if (_playerData.TryAddPower(this)) return;
        gameObject.SetActive(false);
        // summon ui here
       
    }
    private void ResetActive() {
        //_mesh.enabled = _playerData.PowerUp[GetPowerType()];
        //_collider.enabled = _playerData.PowerUp[GetPowerType()];
    }
    public static Type GetPowerType(PowerType type) {
        if (type == PowerType.Running) return typeof(RunState);
        else if (type == PowerType.Jumping) return typeof(JumpState);
        else if (type == PowerType.Dashing) return typeof(DashState);
        else if(type == PowerType.Walling) return typeof(WallGrabState);
        else if(type == PowerType.Frenzying) return typeof(FrenzyState);
        return null;
    }
    private void PowerUp(Type state, bool value) {
        //_playerData.PowerUp[state] = value;
    }
    //private void OnEnable() {
    //    _playerData.OnPowerUpChange += ResetActive;
    //}
    //private void OnDisable() {
    //    _playerData.OnPowerUpChange -= ResetActive;
    //}
}
