using System;
using UnityEngine;
using System.Collections;

public class Player : Character
{
    [SerializeField] private float pushingForce;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnCollisionEnter(Collision other)
    {
        CrashTest(other);
    }

    protected override void CrashBullets(IBullets bullets)
    {
        base.CrashBullets(bullets);
    }

    private void Update()
    {
        _gameManager.playerPosition = transform.position;
    }

    protected override void CrashEntity(Collision other)
    {
        base.CrashEntity(other);
        if (Chara == null) return;
        Chara.Hp -= atk;
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
