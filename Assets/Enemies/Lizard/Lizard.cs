using UnityEngine;
using UnityEngine.Assertions;

public class Lizard : MonoBehaviour
{
    [SerializeField]
    private Transform headPivotTransform;

    private bool isPlayerInTrigger = false;
    private Transform playerTransform;

    private void Awake()
    {
        Assert.IsNotNull(headPivotTransform);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToPlayer = playerTransform.position - headPivotTransform.position;
        float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        float currentAngle = Mathf.Atan2(headPivotTransform.transform.localPosition.y, headPivotTransform.transform.localPosition.x) * Mathf.Rad2Deg;
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        headPivotTransform.transform.Rotate(Vector3.forward, Time.deltaTime * 10);
        Debug.Log(angleDifference);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInTrigger = false;
    }
}
