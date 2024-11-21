using UnityEngine;

public class PlatformParenting : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.transform.parent != transform) {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.transform.parent == transform) {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
