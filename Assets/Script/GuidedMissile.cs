using System.Collections;
using UnityEngine;

public class GuidedMissile : MonoBehaviour, IBullets
{
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float inductiveTime;
    [SerializeField] private float maxDistance = 30f;

    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    private bool _inductive = true;
    private GameManager _gameManager;
    private Rigidbody _rigidbody;
    private float _maxDistanceSqr;

    private void Awake()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
        _maxDistanceSqr = maxDistance * maxDistance;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _rigidbody = GetComponent<Rigidbody>();
        if (_gameManager != null)
            _gameManager.bullets.Add(this);
        StartCoroutine(InductiveSwitching());
        _gameManager.bullets.Add(this);
    }

    private void FixedUpdate()
    {
        if (_inductive)
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
        if (_gameManager != null)
            _gameManager.bullets.Remove(this);
    }

    private IEnumerator InductiveSwitching()
    {
        _inductive = true;
        yield return new WaitForSeconds(inductiveTime);
        _inductive = false;
    }
}