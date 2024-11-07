using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    private Vector3 _to;
    public void DrawLine(Vector3 to) {
        _to = to;
    }
    public void DisableLine() {
        _to = Vector3.zero;
    }
    private void OnDrawGizmos() {
        if (_to == Vector3.zero) return;
        Gizmos.DrawLine(gameObject.transform.position, _to);
    }
}
