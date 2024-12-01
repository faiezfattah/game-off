using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        playerData.checkPoint = gameObject.transform.position; 
        
        if (other.TryGetComponent<PlayerHealth>(out var playerHealth))
            playerHealth.Refill();  
    }
}
