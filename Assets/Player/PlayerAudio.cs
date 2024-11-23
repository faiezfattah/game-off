using System.Collections;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    [SerializeField] private AudioChannel relay;
    public AudioClip walkSfx;
    public AudioClip jumpSfx;
    public AudioClip runSfx;
    public AudioClip dashIn;
    public AudioClip dashOut;
    public AudioClip wallSlide;
    public AudioClip frenzy;
    public AudioClip land;

    private Coroutine _loop;
    public void Play(SfxParams sfx) {
        relay.PlaySfx(sfx);
    }

    public void Play(SfxParams sfx, bool loop) {
        if (!loop) {
            Play(sfx);
            return;
        }

        if (_loop != null) {
            Stop();
            return;
        }
        _loop = StartCoroutine(LoopAudio(sfx));
    }

    public void Stop(string id = default) {
        relay.StopSfx(id);
        
        if (_loop == null) return; 
        StopCoroutine(_loop);
        _loop = null;
    }
    
    private IEnumerator LoopAudio(SfxParams sfx) {
        //Debug.Log("looping audio");
        if (_loop != null) yield break; 
        while (true) {
            Play(sfx);
            yield return new WaitForSeconds(sfx.Clip.length);
        }
        
    }
    
}
