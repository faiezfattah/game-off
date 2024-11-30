using System;
using DG.Tweening;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    [SerializeField]
    private float breakTime = 3f;

    private float _timer;
    private void OnCollisionStay(Collision other) {
        _timer += Time.deltaTime;
        transform.DOShakePosition(breakTime, new Vector3(0.2f,0,0), 20, 90, false, false)
                 .OnComplete(() => gameObject.SetActive(false));
    }
    private void OnCollisionExit(Collision other) {
        _timer = 0;
    }
}
