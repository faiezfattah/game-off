using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FinalCheck : MonoBehaviour, IInteractable {
    [SerializeField] private GameObject            pivot;
    [SerializeField] private PlayerData            playerData;
    [SerializeField] private PowerItem.PowerType wrongType;
    [SerializeField] private Volume                volume;

    [Header("Ending animation values")] [SerializeField]
    private float endingAnimationDuration = 5f;

    public void Interact() {
        Rotate();
    }

    private bool Check() {
        var power = playerData.Powers;
        
        if (power.Contains(PowerItem.GetPowerType(wrongType))) {
            Fail();
            return false;
        }
        
        Success();
        return true;
    }

    private void Rotate() {
        var isCorrect = Check();
        var sequence  = DOTween.Sequence();

        sequence.Append(pivot.transform.DORotateQuaternion(
            Quaternion.Euler(0f, 0f, pivot.transform.rotation.z + 45), 1f));
        sequence.Append(pivot.transform.DORotateQuaternion(
            Quaternion.Euler(0f, 0f, pivot.transform.rotation.z + (isCorrect ? 45 : 0)), 1f));
    }

    private void Success() {
        SuccessAnimation();
    }

    private void SuccessAnimation() {
        volume.profile.TryGet(out ColorAdjustments colorAdjustment);
        volume.profile.TryGet(out ChromaticAberration chromaticAberration);
        volume.profile.TryGet(out FilmGrain filmGrain);
        volume.profile.TryGet(out DepthOfField depthOfField);
        volume.profile.TryGet(out Bloom bloom);
        volume.profile.TryGet(out LensDistortion lensDistortion);

        var sequence = DOTween.Sequence();

        sequence.Join(DOVirtual.Float(0, 1f, endingAnimationDuration, (x) => { filmGrain.intensity.value = x; }));
        sequence.Join(DOVirtual.Float(0, 1f, endingAnimationDuration, (x) => { chromaticAberration.intensity.value = x; }));
        sequence.Join(DOVirtual.Float(0, 200f, endingAnimationDuration, (x) => { bloom.intensity.value = x; }));
        sequence.Join(DOVirtual.Float(0, -0.5f, endingAnimationDuration, (x) => { lensDistortion.intensity.value = x; }));
        
        sequence.Append(DOVirtual.Float(0, 10f, endingAnimationDuration, (x) => { colorAdjustment.postExposure.value = x; }));
        sequence.Join(DOVirtual.Float(10, 0, endingAnimationDuration, (x) => { depthOfField.focusDistance.value = x; }));
        sequence.Join(DOVirtual.Float(1, 0.5f, endingAnimationDuration, (x) => { lensDistortion.scale.value = x; }));

        sequence.Play();
    }

    private void Fail() { }
}