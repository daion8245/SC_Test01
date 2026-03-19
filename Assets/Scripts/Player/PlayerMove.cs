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

        // 감속: LeftShift 입력 시 속도 30%로 감소
        if (Input.GetKey(KeyCode.LeftShift))
            _speed *= 0.3f;
    }
    
    void FixedUpdate()
    {
        // 로컬 방향 → 월드 방향으로 변환 후 속도 적용
        Vector3 moveDir = _rigidbody.transform.forward * _speed;
        _rigidbody.linearVelocity = new Vector3(moveDir.x, _rigidbody.linearVelocity.y, moveDir.z);

        Quaternion rotation = Quaternion.Euler(0, _rotate * Time.fixedDeltaTime, 0);
        _rigidbody.MoveRotation(_rigidbody.rotation * rotation);
    }
}
