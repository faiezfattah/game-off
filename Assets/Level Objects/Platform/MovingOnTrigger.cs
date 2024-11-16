using System.Collections;
using UnityEngine;

public class MovingOnTrigger : MonoBehaviour
{
    [SerializeField] private BoolChannel _trigger;
    [SerializeField] private float _timeToMove = 3f;
    [SerializeField] private GameObject _objectToMove; 
    [SerializeField] private Transform[] _waypoints;
    private bool _isActive = false;
    private int _previousWaypoint = 0;
    private int _currentWaypoint = 1;
    private float _elapsedTime;
    private void FixedUpdate() {
        if (!_isActive && _trigger != null) return;
        _elapsedTime += Time.deltaTime;
        float factor = _elapsedTime / _timeToMove;
        _objectToMove.transform.position = Vector3.Lerp(_waypoints[_previousWaypoint].position, _waypoints[_currentWaypoint].position, factor);
        if (factor >= 1) {
            GetNextWaypoint();
        }
    }
    private void GetNextWaypoint() {
        _previousWaypoint = _currentWaypoint;
        _currentWaypoint = _currentWaypoint + 1;

        if (_currentWaypoint >= _waypoints.Length) {
            _currentWaypoint = 0;
        }

        _elapsedTime = 0;
    }
    void HandleTrigger(bool value) {
        _isActive = value;
    }
    private void OnEnable() {
        if (_trigger == null) return;
        _trigger.OnEventRaised += HandleTrigger;
    }
    private void OnDisable() {
        if (_trigger == null) return;
        _trigger.OnEventRaised -= HandleTrigger;
    }
}
