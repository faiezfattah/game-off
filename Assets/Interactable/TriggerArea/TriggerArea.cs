using System;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] private IToggleableTarget _target;

    private void Start() {
        if (_target == null) Debug.LogWarning("Target not set!");
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        _target.Toggle(true);   
    }
}
