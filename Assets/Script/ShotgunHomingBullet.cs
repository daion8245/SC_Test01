using System.Collections;
using UnityEngine;

public class ShotgunHomingBullet : MonoBehaviour, IBullets
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float scatterDistance = 3.5f;
    [SerializeField] private float pauseDuration = 0.8f;
    [SerializeField] private float homingSpeed = 6f;
    [SerializeField] private float homingTurnSpeed = 120f;
    [SerializeField] private float homingDuration = 3f;
    [SerializeField] private float maxDistance = 30f;

    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    private GameManager _gameManager;
    private Rigidbody _rigidbody;
    private float _maxDistSqr;

    private void Awake()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _rigidbody = GetComponent<Rigidbody>();
        _maxDistSqr = maxDistance * maxDistance;
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

        // --- Homing: 플레이어를 향해 회전하며 추적 ---
        float elapsed = 0f;
        while (elapsed < homingDuration && _gameManager != null)
        {
            Vector3 dir = (_gameManager.playerPosition - transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRot, homingTurnSpeed * Time.fixedDeltaTime);

            _rigidbody.MovePosition(_rigidbody.position + transform.forward * (homingSpeed * Time.fixedDeltaTime));

            if (transform.position.sqrMagnitude > _maxDistSqr)
            {
                Destroy(gameObject);
                yield break;
            }

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

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
