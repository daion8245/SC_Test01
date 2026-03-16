using UnityEngine;

public enum ShotgunBulletState { Scatter, Pause, Homing }

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

    private ShotgunBulletState _state = ShotgunBulletState.Scatter;
    private GameManager _gameManager;
    private Rigidbody _rigidbody;
    private Vector3 _startPos;
    private float _timer;
    private float _scatterDistSqr;
    private float _maxDistSqr;

    private void Awake()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
        
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPos = transform.position;
        _scatterDistSqr = scatterDistance * scatterDistance;
        _maxDistSqr = maxDistance * maxDistance;
        if (_gameManager != null)
            _gameManager.bullets.Add(this);
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case ShotgunBulletState.Scatter:
                ScatterUpdate();
                break;
            case ShotgunBulletState.Pause:
                PauseUpdate();
                break;
            case ShotgunBulletState.Homing:
                HomingUpdate();
                break;
        }
    }

    // --- Scatter: 발사 방향으로 직진, 도착지에 가까울수록 감속 ---
    private void ScatterUpdate()
    {
        float distSqr = (transform.position - _startPos).sqrMagnitude;

        if (distSqr >= _scatterDistSqr)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _state = ShotgunBulletState.Pause;
            _timer = 0f;
            return;
        }

        // t: 0(출발)→1(도착) 으로 갈수록 speed가 줄어든다
        float t = Mathf.Sqrt(distSqr) / scatterDistance;
        float speed = Mathf.Lerp(BulletSpeed, BulletSpeed * 0.1f, t);
        _rigidbody.MovePosition(_rigidbody.position + transform.forward * (speed * Time.fixedDeltaTime));
    }

    // --- Pause: 제자리 정지, 대기시간 후 Homing 전환 ---
    private void PauseUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= pauseDuration)
        {
            _state = ShotgunBulletState.Homing;
            _timer = 0f;
        }
    }

    // --- Homing: 플레이어를 향해 회전하며 추적 ---
    private void HomingUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= homingDuration || _gameManager == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (_gameManager.playerPosition - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRot, homingTurnSpeed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(_rigidbody.position + transform.forward * (homingSpeed * Time.fixedDeltaTime));

        if (transform.position.sqrMagnitude > _maxDistSqr)
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
