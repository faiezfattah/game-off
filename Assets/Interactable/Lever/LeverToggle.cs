using System.Collections.Generic;
using UnityEngine;

public class LeverToggle : MonoBehaviour, IInteractable
{
    private bool _isActive;

    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject[] _object;
    private readonly List<IToggleableTarget> _target = new List<IToggleableTarget>();

    private void Start() {
        foreach (var obj in _object) {
            if (!obj.TryGetComponent<IToggleableTarget>(out var item)) {
                Debug.Log("an item is incompatible! Make sure it is toggleable from " + gameObject.name);
            };
            _target.Add(item);
        }
        Rotate();
    }
    public void Interact() {
        _isActive = !_isActive;
        Rotate();
        if (_object.Length <= 0) {
            Debug.LogError("Target object not set!");
            return;
        }

        foreach (var obj in _target) {
            obj.Toggle(_isActive);
        }
    }
    private void Rotate() {
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, pivot.transform.rotation.z + (_isActive ? 45 : 0));
    }
}
