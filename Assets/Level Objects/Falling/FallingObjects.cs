using System;
using DG.Tweening;
using UnityEngine;

public class FallingObjects : MonoBehaviour {
    [SerializeField]
    private Transform endPos;

    [SerializeField]
    private Transform body;

    [SerializeField]
    private float fallDuration = 1f;

    [SerializeField]
    private float recoveryDuration = 3f;

    [SerializeField]
    private float recoveryDelay = 2f;

    private Vector3 _startPos;
    private Sequence _sequence;
    void Start()
    {
        _startPos = body.position;
        
        _sequence = DOTween.Sequence();
        _sequence.Append(body.transform.DOMove(endPos.position, fallDuration)
                             .SetEase(Ease.OutExpo));
        _sequence.AppendInterval(recoveryDelay);
        _sequence.Append(body.transform.DOMove(_startPos, recoveryDuration)
                             .SetEase(Ease.OutExpo));
        
        _sequence.SetAutoKill(false);
        _sequence.Pause();
    }
    private void Fall() {
        if (!_sequence.IsPlaying()) 
            _sequence.Play();
    }
    private void OnTriggerEnter(Collider other) {
        Fall();
    }
}
