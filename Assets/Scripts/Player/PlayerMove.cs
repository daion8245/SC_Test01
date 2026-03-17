using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float maxRotateSpeed;

    private float _speed;
    private float _rotate;
    private Rigidbody _rigidbody;

    private float _vertical;
    private float _horizontal;

    void Start()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        _vertical = Input.GetAxis("Vertical");
        _horizontal = Input.GetAxis("Horizontal");

        _speed = Mathf.Clamp(moveSpeed * _vertical, -maxMoveSpeed, maxMoveSpeed);
        _rotate = Mathf.Clamp(rotateSpeed * _horizontal, -maxRotateSpeed, maxRotateSpeed);
    }

    void FixedUpdate()
    {
        Vector3 localMovement = new Vector3(0, 0, _speed * Time.fixedDeltaTime);
        Vector3 movement = _rigidbody.transform.TransformDirection(localMovement);
        _rigidbody.MovePosition(_rigidbody.position + movement);

        Quaternion rotation = Quaternion.Euler(0, _rotate * Time.fixedDeltaTime, 0);
        _rigidbody.MoveRotation(_rigidbody.rotation * rotation);
    }
}
