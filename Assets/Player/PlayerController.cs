using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _position;
    private float _dir;

    void Start()
    {
        
    }

    void Update()
    {
        if (_dir !=0) {
            move();   
        }
    }
    private void move() {
        _rigidbody.MovePosition(transform.position + new Vector3(_dir, 0, 0) * Time.deltaTime * 10);
    }

    private void OnMove(float dir) {
        _dir = dir;
    }
    private void OnEnable() {
        _inputReader.MoveEvent += OnMove;
    }
    private void OnDisable() {
        _inputReader.MoveEvent -= OnMove;
    }


}
