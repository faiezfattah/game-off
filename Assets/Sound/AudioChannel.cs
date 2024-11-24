using System;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "sfxChannel", menuName = "Event Channel/sfxChannel")]
public class AudioChannel : ScriptableObject {
    // public event Action<AudioClip, Vector3, string> OnPlaySfx;
    public event Action<SfxParams> OnPlaySfx;
    public event Action<string> OnStopSfx;
    public event Action<AudioClip> OnPlayMusic;
    public event Action OnStopMusic;

    // /// <summary> 
    // /// </summary>
    // /// <param name="clip"></param>
    // /// <param name="position">Audio position in 3d space. if null played globally</param>
    // public void PlaySfx(AudioClip clip, Vector3 position) {
    //     OnPlaySfx?.Invoke(clip, position, default);
    // }
    // public void PlaySfx(AudioClip clip, string id) {
    //     OnPlaySfx?.Invoke(clip, default, id);
    // }
    // public void PlaySfx(AudioClip clip) {
    //     OnPlaySfx?.Invoke(clip, default, default);
    // }

    public void PlaySfx(SfxParams param) {
        OnPlaySfx?.Invoke(param);
    }

    public void StopSfx(string id) {
        OnStopSfx?.Invoke(id);
    }
    
    /// <summary>
    /// play music globally
    /// </summary>
    /// <param name="clip"></param>
    public void PlayMusic(AudioClip clip) {
        OnPlayMusic?.Invoke(clip);
    }

    public void StopMusic() {
        OnStopMusic?.Invoke();
    }
}

