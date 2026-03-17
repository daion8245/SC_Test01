using UnityEngine;

public abstract class BulletBase : MonoBehaviour, IBullets
{
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] protected float maxDistance = 30f;

    public int Damage { get; set; }
    public float BulletSpeed { get; set; }

    protected GameManager GameManager;
    protected Rigidbody Rb;
    protected float MaxDistanceSqr;

    protected virtual void Awake()
    {
        Damage = damage;
        BulletSpeed = bulletSpeed;
        MaxDistanceSqr = maxDistance * maxDistance;
    }

    protected virtual void Start()
    {
        GameManager = GameManager.Instance;
        Rb = GetComponent<Rigidbody>();
        if (GameManager != null)
            GameManager.bullets.Add(this);
    }

    protected virtual void FixedUpdate()
    {
        Move();
        if (transform.position.sqrMagnitude > MaxDistanceSqr)
            Destroy(gameObject);
    }

    protected abstract void Move();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
            ApplyDamage(character);
    }

    public virtual void ApplyDamage(Character character)
    {
        character.Hp -= Damage;
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (GameManager != null)
            GameManager.bullets.Remove(this);
    }
}
