using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class FinalCheck : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject            pivot;
    [SerializeField] private PlayerData            playerData;
    [SerializeField] private PowerItem.PowerType[] correctType = new PowerItem.PowerType[3];
    public void Interact() {
        Rotate();
    }

    private bool Check() {
        var power = playerData.Powers;
        foreach (var item in correctType) {
            if (power.Contains(PowerItem.GetPowerType(item))) continue;
            Fail();
            return false;
        }
        Sucess();
        return true;
    }
    private void Rotate() {
        var isCorrect = Check();
        var sequence = DOTween.Sequence();
        
        sequence.Append(pivot.transform.DORotateQuaternion(
            Quaternion.Euler(0f, 0f, pivot.transform.rotation.z + 45), 1f));
        sequence.Append(pivot.transform.DORotateQuaternion(
            Quaternion.Euler(0f, 0f, pivot.transform.rotation.z + (isCorrect ? 45 : 0)), 1f));
    }

    private void Sucess() {
        
    }

    private void Fail() {
        
    }
}
