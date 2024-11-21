using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IToggleableTarget
{
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private float _timeToMove = 3f;
    [SerializeField] private GameObject _objectToMove; 
    [SerializeField] private Transform[] _waypoints;

    private Tweener _tween;
    private int _currentIndex = 1;
    private int _nextIndex;

    protected bool isActive = false;
    private void Start() {
        if (!isActive && isTriggered) return;
         Move();
    }
    private void Move() {
        //if (_tween != null) return;

        // i miss u gsap muah
        _tween = _objectToMove.transform.DOMove(_waypoints[_currentIndex].position, _timeToMove)
            .SetEase(Ease.InOutQuad)
            .SetAutoKill(false)
            .OnComplete(() => {
                HandleComplete();
            });
    }
    private void HandleComplete() {
        _currentIndex = _nextIndex;
        _nextIndex = (_currentIndex + 1) % _waypoints.Length; // such elegancy omg.

        if (!isTriggered || isActive) {
            Move();
        }
    }
    public void Toggle(bool value) {
        isActive = value;

        if (value) {
            _tween?.Kill();
            Move();
        }
        else {
            _tween?.Pause();
        }
    }
    public void OnEnable() {
        _tween?.Play();
    }
    public void OnDisable() {
        _tween?.Pause();
    }
}
