using System;
using System.Collections;
using UnityEngine;

public class GuidedMissile : MonoBehaviour, IBullets
{
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float inductiveTime;

    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    private bool _inductive = true;
    private GameManager _gameManager;
    private Vector3 _nextRotation;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(InductiveSwitching());
    }

    private void Update()
    {
        ((IBullets)this).BulletMovement();
    }

    private void FixedUpdate()
    {
        if(_inductive)
            transform.LookAt(GameManager.Instance.playerPosition);
        _rigidbody.MovePosition(_rigidbody.position + transform.forward * (bulletSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Character character))
        {
            ((IBullets)this).ApplyDamage(character);
        }
    }

    void IBullets.ApplyDamage(Character character)
    {
        character.Hp -= Damage;
    }

    void IBullets.BulletMovement()
    {
        if (!_inductive)
        {
            return;
        }
        _nextRotation = GameManager.Instance.playerPosition;
    }

    private IEnumerator InductiveSwitching()
    {
        _inductive = true;
        yield return new WaitForSeconds(inductiveTime);
        _inductive = false;
    }
}