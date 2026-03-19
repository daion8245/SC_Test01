using UnityEngine;

public class Player : Character
{
    [SerializeField] private float pushingForce;
    private GameManager _gameManager;
    private UiManager _uiManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _uiManager = UiManager.Instance;
        _gameManager.player = this;
        _uiManager.player = this;
    }

    private void OnCollisionEnter(Collision other)
    {
        CrashTest(other);
    }

    private void Update()
    {
        _gameManager.playerPosition = transform.position;
    }

    protected override void CrashEntity(Collision other)
    {
        base.CrashEntity(other);
        if (Chara == null) return;
        Chara.TakeDamage(atk);
        PushAwayEntity(other);
    }

    private void PushAwayEntity(Collision collision)
    {
        Rigidbody otherRb = collision.rigidbody;
        if (otherRb != null)
        {
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
            otherRb.AddForce(pushDirection * pushingForce, ForceMode.Impulse);
        }
    }
}
