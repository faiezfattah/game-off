using System;
using DG.Tweening;
using UnityEngine;

public class PotionSolution : MonoBehaviour, IInteractable {
    [SerializeField] private PotionMixPot         mixPot;
    [SerializeField] private PotionMixPot.Potions type;
    [SerializeField] private float                maxRaycastDistance;
    [SerializeField] private LayerMask            mixingPotLayerMask;
    [SerializeField] private float                rotateDuration;

    private Sequence _sequence;
    private void Start() {
    }

    public void Interact() {
        Rotate();
    }

    private void Cast() {
        if (Physics.Raycast(transform.position, Vector3.down, maxRaycastDistance, mixingPotLayerMask)) {
            mixPot.MixPotion(type);
        }
    }

    private void Rotate() {
        _sequence = DOTween.Sequence();
        _sequence.Append(gameObject.transform.DORotateQuaternion(Quaternion.Euler(-90, 0, 0), rotateDuration)
                                   .SetEase(Ease.InOutQuad));
        _sequence.AppendCallback(Cast);
        _sequence.Append(gameObject.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), rotateDuration)
                                   .SetEase(Ease.InOutQuad));
    }
}