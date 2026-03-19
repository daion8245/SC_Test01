using System.Collections;
using UnityEngine;

public class Shelling : MonoBehaviour, IBullets
{
    public int Damage { get; set; }
    public float BulletSpeed { get; set; }
    public Vector3 LookPosition { get; set; }
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float effectDisTime = 2f;
    
    public void ForcedInduction()
    {
        _rigidbody.MovePosition(LookPosition);
    }

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
        LookPosition = GameManager.Instance.playerPosition;
    }

    private IEnumerator ShellingStart()
    {
        float elapsed = 0f;
        while (elapsed < inductiveTime)
        {
            LookPosition = GameManager.Instance.playerPosition;
            _rigidbody.MovePosition(LookPosition);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(2f);

        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Character character))
                character.TakeDamage(Damage);
        }
        Destroy(gameObject);
    }

    public void ApplyDamage(Character character)
    {
        character.TakeDamage(Damage);
    }
    
    protected void EffectGeneration(Collider collision)
    {
        if (effectPrefab == null)
            return;
        
        Vector3 explosionPosition = transform.position;
        GameObject effect = Instantiate(effectPrefab, explosionPosition, Quaternion.identity);
        Destroy(effect, effectDisTime);
        Destroy(gameObject);
    }

}
