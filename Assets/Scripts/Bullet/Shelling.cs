using System.Collections;
using UnityEngine;

public class Shelling : MonoBehaviour, IBullets
{
    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float inductiveTime;
    [SerializeField] private float damageRadius = 2f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Damage = damage;
        BulletSpeed = bulletSpeed;
        StartCoroutine(ShellingStart());
    }

    private IEnumerator ShellingStart()
    {
        float elapsed = 0f;
        while (elapsed < inductiveTime)
        {
            _rigidbody.MovePosition(GameManager.Instance.playerPosition);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(2f);

        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Character character))
                character.Hp -= Damage;
        }
        Destroy(gameObject);
    }

    public void ApplyDamage(Character character)
    {
        character.Hp -= Damage;
    }
}
