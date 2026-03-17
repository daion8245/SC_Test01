using UnityEngine;

public class StraightBullet : BulletBase
{
    protected override void Move()
    {
        Rb.MovePosition(Rb.position + transform.forward * (BulletSpeed * Time.fixedDeltaTime));
    }
}
