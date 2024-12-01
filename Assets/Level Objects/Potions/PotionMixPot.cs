using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PotionMixPot : MonoBehaviour, IToggleableTarget {
    [SerializeField] private Transform  endpoint;
    [SerializeField] private Transform  pot;
    [SerializeField] private bool       active;
    [SerializeField] private float      moveTime            = 5;
    [SerializeField] private float      maxPotionMix        = 3;
    [SerializeField] private float      changeColorDuration = 1f;
    [SerializeField] private Light      potionLight;
    [SerializeField] private GameObject toHide;


    [Header("Correct Mix")] [SerializeField]
    private Potions[] correctMix = new Potions[2];

    private Tweener       _tweener;
    private List<Vector3> _positions;
    private Vector3       _current;
    private Renderer      _renderer;

    public enum Potions {
        Red,
        Green,
        Blue,
    }

    private List<Potions> _currentHold = new List<Potions>();

    private void Start() {
        _positions = new List<Vector3>();
        _positions.Add(pot.position);
        _positions.Add(endpoint.position);
        _current  = _positions[1];
        _renderer = pot.GetComponent<Renderer>();
        Material mat = _renderer.material;
        // foreach(string prop in mat.GetTexturePropertyNames())
        // {
        //     Debug.Log(prop);
        // }
        // foreach(string prop in mat.GetPropertyNames(MaterialPropertyType.Float))
        // {
        //     Debug.Log(prop);
        // }
        // foreach(string prop in mat.GetPropertyNames(MaterialPropertyType.Texture))
        // {
        //     Debug.Log(prop);
        // }
        // foreach(string prop in mat.GetPropertyNames(MaterialPropertyType.Int))
        // {
        //     Debug.Log(prop);
        // }
        // foreach(string prop in mat.GetPropertyNames(MaterialPropertyType.Matrix))
        // {
        //     Debug.Log(prop);
        // }
        // foreach(string prop in mat.GetPropertyNames(MaterialPropertyType.Vector))
        // {
        //     Debug.Log(prop);
        // }
    }

    public void MixPotion(Potions type) {
        if (_currentHold.Count >= maxPotionMix) {
            Debug.Log("potion maxed!");
            return;
        }

        _currentHold.Add(type);
        Debug.Log($"Potion added: {type}");
        ChangeColor();

        if (CheckMix()) {
            toHide.SetActive(false);
            active = false;
        }
    }

    private bool CheckMix() {
        foreach (var item in correctMix) {
            if (!_currentHold.Contains(item)) return false;
        }

        return true;
    }

    private void ChangeColor() {
        int   r     = _currentHold.Contains(Potions.Red) ? 1 : 0;
        int   g     = _currentHold.Contains(Potions.Green) ? 1 : 0;
        int   b     = _currentHold.Contains(Potions.Blue) ? 1 : 0;
        Color color = new Color(r, g, b);
        potionLight.DOColor(color, changeColorDuration);
    }

    private void Move() {
        _tweener = pot.transform.DOMove(_current, moveTime)
                      .SetEase(Ease.Linear)
                      .SetAutoKill(false)
                      .OnComplete(() => {
                          _current = _current == _positions[0] ? _positions[1] : _positions[0];
                          if (!active) return;
                          Move();
                      });
    }

    public void Toggle(bool value) {
        active = value;

        if (value) {
            _tweener?.Kill();
            Move();
            potionLight.DOColor(Color.white, changeColorDuration);
        }
        else {
            _tweener?.Pause();
            potionLight.DOColor(Color.white, changeColorDuration);
            _currentHold.Clear();
        }
    }

    private void OnEnable() {
        _tweener?.Play();
    }

    private void OnDisable() {
        _tweener?.Kill();
    }
}