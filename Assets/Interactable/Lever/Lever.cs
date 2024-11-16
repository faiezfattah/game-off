using UnityEngine;

public class Lever : Interactable
{
    private bool _isActive = false;
    private GameObject _leverBody;
    [SerializeField] private BoolChannel _relay;

    private void Start() {
        _leverBody = gameObject;
    }
    public override void Interact() {
        _isActive = !_isActive;
        _leverBody.transform.rotation = Quaternion.Euler(0f, 0f, _isActive ? -45f : 45f);
        _relay.RaiseEvent(_isActive);
    }
}
