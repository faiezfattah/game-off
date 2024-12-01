using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerSceneChange : MonoBehaviour {
    public UnityEvent<LevelLoader.SceneIndex> listener;
    public LevelLoader.SceneIndex             index;
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        listener?.Invoke(index);
    }
}