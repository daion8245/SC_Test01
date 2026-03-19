using UnityEngine;

public abstract class BulletBase : MonoBehaviour, IBullets
{
    [SerializeField] protected int damage;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float maxDistance = 30f;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float effectDisTime = 2f;

    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    protected Rigidbody _rigidbody;
    protected float _maxDistanceSqr;
    private Vector3 _spawnPosition;

    protected virtual void Awake()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
        _maxDistanceSqr = maxDistance * maxDistance;
    }

    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _spawnPosition = transform.position;
    }

    protected void CheckMaxDistance()
    {
        if ((transform.position - _spawnPosition).sqrMagnitude > _maxDistanceSqr)
            Destroy(gameObject);
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        EffectGeneration(other);
        
        if (other.TryGetComponent(out Character character))
            ApplyDamage(character);
    }

    public void ApplyDamage(Character character)
    {
        character.TakeDamage(Damage);
        Destroy(gameObject);
    }

    public Vector3 LookPosition { get; set; }
    public void ForcedInduction()
    {
        transform.LookAt(LookPosition);
    }
}
