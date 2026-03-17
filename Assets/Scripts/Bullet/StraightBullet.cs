using UnityEngine;

public class StraightBullet : BulletBase
{
    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + transform.forward * (BulletSpeed * Time.fixedDeltaTime));
        CheckMaxDistance();
    }
}
