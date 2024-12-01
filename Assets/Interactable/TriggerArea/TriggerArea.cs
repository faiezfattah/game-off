using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour {
    public UnityEvent listener;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        listener?.Invoke();
    }
}