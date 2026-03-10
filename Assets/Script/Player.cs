using System;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float pushingForce;

    private void OnCollisionEnter(Collision other)
    {
        CrashTest(other);
    }

    protected override void CrashBullets(IBullets bullets)
    {
        base.CrashBullets(bullets);
    }

    protected override void CrashEntity(Collision other)
    {
        base.CrashEntity(other);
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
