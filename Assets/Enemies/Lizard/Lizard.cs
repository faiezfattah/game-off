using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class Lizard : MonoBehaviour {
    [SerializeField]
    private Transform headPivotTransform;

    [SerializeField]
    private float idleRotationDuration = 0.5f;

    [SerializeField]
    float SlowDownDuration = 1f;

    [SerializeField]
    private float distance = 5f;

    [SerializeField]
    private Sprite idleSprite;

    [SerializeField]
    private Sprite attackSprite;

    [SerializeField]
    private float attackCooldown = 1;

    [SerializeField]
    private SpriteRenderer head;

    private static Transform _playerTransform;
    private        Tweener   _currentTween;
    private        Coroutine _routine;
    private Coroutine _slowRoutine;

    private void Awake() {
        Assert.IsNotNull(headPivotTransform);

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        RotateIdle();
    }

    // private void Update()
    // {
    //     
    //     
    //     // float currentAngle = Mathf.Atan2(headPivotTransform.transform.localPosition.y, headPivotTransform.transform.localPosition.x) * Mathf.Rad2Deg;
    //     // float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
    //     //headPivotTransform.transform.Rotate(Vector3.forward, Time.deltaTime * 10);
    //     // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
    //     //Debug.Log(targetRotation);
    //     
    //     // code from turret game
    //     // Vector3 direction = target.position - transform.position;
    //     // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
    //     // Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //     // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, data.rotationSpeed * Time.deltaTime);
    //     // 1. get directional vector 
    //     // 2. get angle diff 
    //     // 3. make quaternion cuz unity succ
    //     // 4. apply quaternion
    // }

    private void RotateIdle() {
        // Vector3 directionToPlayer = (_playerTransform.position - headPivotTransform.position).normalized;
        // float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        // targetAngle = Mathf.Clamp(targetAngle, -90f, 90f);
        Quaternion targetRotation = Quaternion.AngleAxis(headPivotTransform.rotation.z + 180f, Vector3.forward);

        _currentTween?.Kill();
        if (_routine != null) {
            StopCoroutine(_routine);
            _routine = null;
        }

        _currentTween = headPivotTransform.DORotateQuaternion(targetRotation, idleRotationDuration)
                                          .SetEase(Ease.Linear)
                                          .SetLoops(-1, LoopType.Yoyo);
    }

    private void RotateToPlayer() {
        Vector3 directionToPlayer = (_playerTransform.position - headPivotTransform.position).normalized;
        float   targetAngle       = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, -90, 0f);
        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

        _currentTween?.Kill();

        _currentTween = headPivotTransform.DORotateQuaternion(targetRotation, 0.1f)
                                          .SetEase(Ease.Linear);
    }


    private IEnumerator Attack() {
        RotateToPlayer();

        head.sprite = attackSprite;
        Cast();
        yield return new WaitForSeconds(0.5f);
        head.sprite = idleSprite;

        yield return new WaitForSeconds(attackCooldown);

        _routine = null;
    }

    private void Cast() {
        if (!Physics.Raycast(headPivotTransform.position, headPivotTransform.forward, out var hit, distance,
                LayerMask.GetMask("Player"))) return;
        if (hit.collider && hit.collider.CompareTag("Player")) {
            if (!hit.collider.TryGetComponent<PlayerController>(out var player)) return;
            if (_slowRoutine != null) {
                StopCoroutine(_slowRoutine);
            }
            _slowRoutine = StartCoroutine(ApplySlow(player));
        }
    }

    private IEnumerator ApplySlow(PlayerController player) {
        var initialSpeed = player.settings.initialSpeedModifier;
        if (!Mathf.Approximately(player.settings.speedModifier, initialSpeed)) {
            player.settings.speedModifier /= 0.5f;
        }
        yield return new WaitForSeconds(SlowDownDuration);
        player.settings.speedModifier = initialSpeed;
    }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player") && _routine == null)
            _routine = StartCoroutine(Attack());
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
            RotateIdle();
    }
}