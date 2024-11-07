using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRelativeStartPointInputProcessor : InputProcessor<Vector2> {
    public bool normalize;
    private Vector2 _startPosition;
    private Vector2 _currentPosition;
    private Vector2 _resultVector;
    public override Vector2 Process(Vector2 value, InputControl control) {
        if (_startPosition == null) {
            _startPosition = value;
        }
        _currentPosition = value;
        _resultVector = _currentPosition - _startPosition;
        return normalize ? _resultVector.normalized : _resultVector;
    }
}
