using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    private Vector3 _to;
    private Vector3 _aimFrom;
    private Vector3 _aimTo;
    public void DrawDashingLine(Vector3 to) {
        _to = to;
    }
    public void DisableDashingLine() {
        _to = Vector3.zero;
    }
    private void OnDrawGizmos() {
        if (_to != Vector3.zero) {
            Gizmos.DrawLine(gameObject.transform.position, _to);
        }
    }
}
