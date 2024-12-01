using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject[] images;
    [SerializeField] private GameObject   cover;
    public                   UnityEvent<LevelLoader.SceneIndex>  OnEnd;

    private Image blackScreen;
    private void Awake() {
        blackScreen = cover.GetComponent<Image>();
        foreach (var img in images) {
            img.SetActive(false);
        }
    }

    void Start()
    {
        var sequence = DOTween.Sequence();
        foreach (var img in images) {
            sequence.AppendCallback(() => img.SetActive(true));
            sequence.Append(blackScreen.DOColor(new Color(0, 0, 0, 0), 1)).SetEase(Ease.Linear);
            sequence.AppendInterval(3f);
            sequence.Append(blackScreen.DOColor(new Color(0, 0, 0, 1), 1)).SetEase(Ease.Linear);
            sequence.AppendCallback(() => img.SetActive(false));
        }
        sequence.AppendCallback(() => OnEnd?.Invoke(LevelLoader.SceneIndex.Stage1));
        sequence.Play();
    }
}
