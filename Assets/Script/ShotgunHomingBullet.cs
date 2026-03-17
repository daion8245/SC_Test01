using System.Collections;
using UnityEngine;

public class ShotgunHomingBullet : MonoBehaviour, IBullets
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float scatterDistance = 3.5f;
    [SerializeField] private float pauseDuration = 0.8f;
    [SerializeField] private float maxDistance = 30f;
    [SerializeField] private float inductiveTime = 1f;

    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    private GameManager _gameManager;
    private Rigidbody _rigidbody;
    private float _maxDistanceSqr;
    private bool _inductive;
    private bool _fixedUpdateActive;

    private void Awake()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _rigidbody = GetComponent<Rigidbody>();
        _maxDistanceSqr = maxDistance * maxDistance;
        if (_gameManager != null)
            _gameManager.bullets.Add(this);

        StartCoroutine(BulletRoutine());
    }

    private IEnumerator BulletRoutine()
    {
        // --- Scatter: 발사 방향으로 직진, 도착지에 가까울수록 감속 ---
        Vector3 startPos = transform.position;
        float scatterDistSqr = scatterDistance * scatterDistance;

        while (true)
        {
            float distSqr = (transform.position - startPos).sqrMagnitude;
            if (distSqr >= scatterDistSqr)
                break;

            float t = Mathf.Sqrt(distSqr) / scatterDistance;
            float speed = Mathf.Lerp(BulletSpeed, BulletSpeed * 0.1f, t);
            _rigidbody.MovePosition(_rigidbody.position + transform.forward * (speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        _rigidbody.linearVelocity = Vector3.zero;

        // --- Pause: 제자리 정지, 대기시간 후 Homing 전환 ---
        yield return new WaitForSeconds(pauseDuration);

        // --- Homing: FixedUpdate에서 플레이어 추적 시작 ---
        _inductive = true;
        _fixedUpdateActive = true;
        
        yield return new WaitForSeconds(inductiveTime);
        
        _inductive = false;
    }

    private void FixedUpdate()
    {
        if (!_fixedUpdateActive) return;

        if (_inductive && _gameManager)
            transform.LookAt(_gameManager.playerPosition);

        _rigidbody.MovePosition(_rigidbody.position + transform.forward * (BulletSpeed * Time.fixedDeltaTime));

        if (transform.position.sqrMagnitude > _maxDistanceSqr)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
            ApplyDamage(character);
    }

    public void ApplyDamage(Character character)
    {
        character.Hp -= Damage;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_gameManager == null || _gameManager.bullets == null)
            return;
        _gameManager.bullets.Remove(this);
    }
}
