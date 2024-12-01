using System;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip music;
    [Tooltip("Optional, insert only when wanting the music to play on trigger")]
    [SerializeField] private TriggerArea trigger;

    private void Start() {
        if (music == null) Debug.LogWarning("Current stage music missing!");
        if (trigger != null) trigger.listener.AddListener(PlayMusicOnTrigger);
        PlayMusic();
    }

    private void PlayMusic() {
        if (trigger != null) return;
        soundManager.PlayMusic(music);
    }

    public void PlayMusicOnTrigger() {
        soundManager.PlayMusic(music);
    }
}