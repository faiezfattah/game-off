using UnityEngine;

public class PlayerVisualizer : MonoBehaviour {
    [SerializeField] private LineRenderer _line;
    private Vector2 _target;
    public float radius;
    public float aimLength;
    public GameObject pointer;
    public void DrawDashingLine(Vector2 to) {
        _target = to;
    }
    public void DisableDashingLine() {
        _target = Vector2.zero;
    }
    private void Update() {        
        _line.SetPosition(0, transform.position);
        if (_target != Vector2.zero) _line.SetPosition(1, (Vector2) transform.position + Vector2.ClampMagnitude(_target, aimLength));
        else _line.SetPosition(1, transform.position);
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(pointer.transform.position, radius);
    }
}
