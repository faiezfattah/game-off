using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private string unsafeTag = "Unsafe";
    [SerializeField][Range(0, 1)] private float lineancy = 0.7f;
    private void Start() {
        gameObject.tag = unsafeTag;
    }

    private void OnCollisionEnter(Collision collision) {
        ContactPoint contact = collision.GetContact(0);

        //check if the contact point is on the bottom (0, -1, 0).
        //break immidiately
        if (contact.normal.y > -lineancy) return;

        if (!collision.gameObject.TryGetComponent<PlayerHealth>(out var player)) return;
        player.TryReduce(damage);
    }
}
