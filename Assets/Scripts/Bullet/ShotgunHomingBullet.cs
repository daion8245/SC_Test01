using System.Collections;
using UnityEngine;

public class ShotgunHomingBullet : BulletBase
{
    [SerializeField] private float scatterDistance = 3.5f;
    [SerializeField] private float pauseDuration = 0.8f;
    [SerializeField] private float inductiveTime = 1f;

    private bool _inductive;
    private bool _fixedUpdateActive;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BulletRoutine());
    }

    // BulletBase.FixedUpdate는 BulletRoutine이 제어권을 가져가는 동안 Move()를 호출함
    // _fixedUpdateActive 플래그로 Homing 단계 이전에는 코루틴이 직접 이동을 처리
    protected override void Move()
    {
        if (!_fixedUpdateActive) return;

        if (_inductive && GameManager)
            transform.LookAt(GameManager.playerPosition);

        Rb.MovePosition(Rb.position + transform.forward * (BulletSpeed * Time.fixedDeltaTime));
    }

    private IEnumerator BulletRoutine()
    {
        // Scatter: 발사 방향으로 직진, 도착지에 가까울수록 감속
        Vector3 startPos = transform.position;
        float scatterDistSqr = scatterDistance * scatterDistance;

        while (true)
        {
            float distSqr = (transform.position - startPos).sqrMagnitude;
            if (distSqr >= scatterDistSqr)
                break;

            float t = Mathf.Sqrt(distSqr) / scatterDistance;
            float speed = Mathf.Lerp(BulletSpeed, BulletSpeed * 0.1f, t);
            Rb.MovePosition(Rb.position + transform.forward * (speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        Rb.linearVelocity = Vector3.zero;

        // Pause: 대기
        yield return new WaitForSeconds(pauseDuration);

        // Homing: FixedUpdate에서 플레이어 추적
        _inductive = true;
        _fixedUpdateActive = true;

        yield return new WaitForSeconds(inductiveTime);

        _inductive = false;
    }
}
