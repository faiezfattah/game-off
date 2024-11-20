using DG.Tweening;
using UnityEngine;

public class Bouncing :MonoBehaviour
{
    protected Vector3 startPosition;
    [SerializeField]
    protected Vector3 endPosition;
    [SerializeField]
    protected float speed = 3f;

    protected Tween tween;

    private void OnDrawGizmosSelected()
    {
        if (endPosition != null)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(transform.position, endPosition);
        }
    }

    private void Awake()
    {
        startPosition = transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        tween = transform.DOMove(endPosition, speed).SetLoops(-1, LoopType.Yoyo);
    }
}
