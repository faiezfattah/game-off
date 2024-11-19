using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IToggleableTarget
{
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private float _timeToMove = 3f;
    [SerializeField] private GameObject _objectToMove; 
    [SerializeField] private Transform[] _waypoints;
    private int _previousWaypoint = 0;
    private int _currentWaypoint = 1;
    private float _elapsedTime;

    protected bool isActive = false;
    private void FixedUpdate() {
        if (!isActive && isTriggered) return;
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
    public void Toggle(bool value) {
        isActive = value;
    }
}
