using UnityEngine;

public class Checkpoint : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerData playerData;

    public void Interact() {
        playerData.checkPoint = gameObject.transform.position; 
    }
}
