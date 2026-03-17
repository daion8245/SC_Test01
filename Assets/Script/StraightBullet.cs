using System;
using UnityEngine;

public class StraightBullet : MonoBehaviour, IBullets
{
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    
    private Rigidbody _rigidbody;
    [SerializeField] private float maxDistanceSqr;
    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    private void Start()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + transform.forward * (BulletSpeed * Time.fixedDeltaTime));
        
        if (transform.position.sqrMagnitude > maxDistanceSqr)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character character))
        {
            ApplyDamage(character);
        }
    }

    public void ApplyDamage(Character character)
    {
        character.Hp -= Damage;
        Destroy(gameObject);
    }
}
