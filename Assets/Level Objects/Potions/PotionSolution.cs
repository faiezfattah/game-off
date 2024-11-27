using DG.Tweening;
using UnityEngine;

public class PotionSolution : MonoBehaviour, IInteractable {
    [SerializeField] private PotionMixPot         mixPot;
    [SerializeField] private PotionMixPot.Potions type;
    [SerializeField] private float                maxRaycastDistance;
    [SerializeField] private LayerMask            mixingPotLayerMask;
    [SerializeField] private float                rotateDuration;

    public void Interact() {
        Rotate();
    }

    private void Cast() {
        if (Physics.Raycast(transform.position, Vector3.down, maxRaycastDistance, mixingPotLayerMask)) {
            mixPot.MixPotion(type);
        }
    }

    private void Rotate() {
        // gameObject.transform.DORotateQuaternion(Quaternion.Euler(-90, 0, 0), rotateDuration)
        //           .From(Quaternion.Euler(0,0,0))
        //           .SetEase(Ease.InOutQuad)
        //           .OnComplete(Cast)
        //           .SetLoops(2, LoopType.Yoyo);
        
        // manually do the yoyo after func callback. forever hate dotween documentation
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(gameObject.transform.DORotateQuaternion(Quaternion.Euler(-90, 0, 0), rotateDuration)
                                    .SetEase(Ease.InOutQuad));
        mySequence.AppendCallback(Cast);
        mySequence.Append(gameObject.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), rotateDuration)
                                    .SetEase(Ease.InOutQuad));
    }
}