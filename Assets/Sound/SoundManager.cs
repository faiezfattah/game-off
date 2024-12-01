using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour {
    [FormerlySerializedAs("sfxRelay")] [SerializeField] private AudioChannel audioRelay;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private int maxSfxCapacity = 100;
    
    private ObjectPool<AudioSource>       _sfxPlayer; 
    private Dictionary<string, AudioSource> _trackedSound = new Dictionary<string, AudioSource>();
    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (!playerTransform) {Debug.Log("playerTransform is null");}

        if (musicSource == null) {
            var musicObj = new GameObject("Music Player");
            musicSource = musicObj.AddComponent<AudioSource>();
            musicObj.transform.SetParent(playerTransform);
        }
        _sfxPlayer = new ObjectPool<AudioSource>(SfxCreation, SfxUse, SfxReturn, SfxDestroy, false, 1, maxSfxCapacity);
    }

    public void PlayMusic(AudioClip clip) {
        musicSource.transform.SetParent(playerTransform);
        musicSource.clip = clip;
        musicSource.loop = true;
        
        musicSource.Play();
    }
    private void PlaySfx(SfxParams sfx) {
        StartCoroutine(PlaySfxRoutine(sfx));
    }

    private void StopSfx(string id) {
        if (!_trackedSound.TryGetValue(id, out var player)) return;
        
        player.Stop();
        _sfxPlayer.Release(player);
        _trackedSound.Remove(id);
    }
    private IEnumerator PlaySfxRoutine(SfxParams sfx) {
        //get from pool, assign params, play, wait, release
        var player = _sfxPlayer.Get();
        player ??= _sfxPlayer.Get();
        
        player.transform.position = sfx.Position ?? playerTransform.position; // null coalesing something something. 
        player.volume = sfx.Volume ?? 1;
        player.pitch = sfx.Pitch ?? 1;
        if (sfx.Id != null) _trackedSound[sfx.Id] = player;
        
        player.PlayOneShot(sfx.Clip);
        player.Play();
        
        float time = sfx.Clip.length;
        yield return new WaitForSeconds(time);
        
        if (player.isPlaying) player.Stop();
        _sfxPlayer?.Release(player);
    }
    private AudioSource SfxCreation() {
        var sfxObject   = new GameObject("sfxPlayer");
        sfxObject.transform.SetParent(transform); // make the editor cleaner like my mind ykno ykno
        
        var sfxSource  = sfxObject.AddComponent<AudioSource>();
        sfxSource.spatialBlend = 1;
        sfxSource.playOnAwake  = false;
        sfxSource.rolloffMode  = AudioRolloffMode.Linear;
        return sfxSource;
    }
    private void SfxUse(AudioSource sfxSource) {
        if (sfxSource == null) {
            Debug.Log("sfx clip is null ");
            return;
        }
        sfxSource.gameObject.SetActive(true);
    }

    private void SfxReturn(AudioSource sfxSource) {
        if (sfxSource == null) {
            Debug.Log("sfx clip is null ");
            return;
        }
        sfxSource.gameObject.SetActive(false);
    }

    private void SfxDestroy(AudioSource sfxSource) {
        Destroy(sfxSource);
    }
    private void OnEnable() {
        audioRelay.OnPlaySfx   += PlaySfx;
        audioRelay.OnPlayMusic += PlayMusic;
        audioRelay.OnStopSfx   += StopSfx;
    }    
    private void OnDisable() {
        audioRelay.OnPlaySfx   -= PlaySfx;
        audioRelay.OnPlayMusic -= PlayMusic;
        audioRelay.OnStopSfx   -= StopSfx;

    }
    private void OnDestroy() {
        _sfxPlayer?.Dispose();
    }
}
