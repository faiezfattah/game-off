using System;
using DG.Tweening;
using UnityEngine;

public class Spider : Bouncing {
    [SerializeField]
    private int damage = 1;
    protected override void Start()
    {
        base.Start();
        tween.SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider hit) {
        if (hit.CompareTag("Player")) {
            hit.TryGetComponent<PlayerHealth>(out var health);
            health.TryReduce(damage);
        }
    }
}
