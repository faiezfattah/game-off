using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using UnityEngine;

public class BigSpider : MonoBehaviour {
    [SerializeField] private Transform[] waypoint;
    [SerializeField] private Transform   spider;
    [SerializeField] private float       speed            = 10f;
    [SerializeField] private float       waypointInterval = 1f;
    [SerializeField] private GameObject  toHideAfter;


    private void Start() {
        StartChase();
    }

    private void OnTriggerEnter(Collider other) {
        StartChase();
    }

    private void StartChase() {
        var timeline     = DOTween.Sequence();
        var prevPosition = spider.position;
        var prevRotation = waypoint[0].position - spider.position; // dir vector
        // timeline.Pause();

        // foreach (var point in waypoint) {
        //     // duration = speed * distance
        //     var dist = Vector3.Distance(prevPosition, point.position);
        //
        //     var next = point.position - prevRotation;
        //
        //     timeline.Append(spider.DOMove(point.position, dist / speed).SetEase(Ease.Linear))
        //             .Join(spider.DORotateQuaternion(Quaternion.Euler(0, 0, diff), 0.5f));
        //     timeline.AppendInterval(waypointInterval);
        //
        //     prevPosition = point.position;
        //     prevRotation = diff;
        // }

        for (int i = 0; i < waypoint.Length; i++) {
            var dist = Vector3.Distance(prevPosition, waypoint[i].position);

            Vector3 direction = waypoint[i].position - prevPosition;
            float   angle     = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            timeline.Append(spider.DOMove(waypoint[i].position, dist / speed).SetEase(Ease.Linear))
                    .Join(spider.DORotateQuaternion(Quaternion.Euler(0, 0, angle + 90), 0.5f));
            timeline.AppendInterval(waypointInterval);

            prevPosition = waypoint[i].position;
        }

        timeline.AppendCallback(() => { toHideAfter.SetActive(false); });
        timeline.Play();
    }
}