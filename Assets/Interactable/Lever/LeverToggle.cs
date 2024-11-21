using System.Collections.Generic;
using UnityEngine;

public class LeverToggle : MonoBehaviour, IInteractable
{
    private bool _isActive = false;
    private GameObject _leverBody;

    public GameObject[] _object;
    private List<IToggleableTarget> _target = new List<IToggleableTarget>();

    private void Start() {
        _leverBody = gameObject;
        foreach (var obj in _object) {
            if (!obj.TryGetComponent<IToggleableTarget>(out var item)) {
                Debug.Log("an item is incompatible! Make sure it is toggleable");
            };
            _target.Add(item);
        }
    }
    public void Interact() {
        _isActive = !_isActive;
        _leverBody.transform.rotation = Quaternion.Euler(0f, 0f, _isActive ? -45f : 45f);

        if (_object.Length <= 0) {
            Debug.LogError("Target object not set!");
            return;
        }

        foreach (IToggleableTarget obj in _target) {
            obj.Toggle(_isActive);
        }
    }
}