using System.Collections;
using UnityEngine;

public class ShotgunHomingBullet : BulletBase
{
    [SerializeField] private float scatterDistance = 3.5f;
    [SerializeField] private float pauseDuration = 0.8f;
    [SerializeField] private float inductiveTime = 1f;

    private GameManager _gameManager;
    private bool _inductive;
    private bool _fixedUpdateActive;

    protected override void Start()
    {
        base.Start();
        _gameManager = GameManager.Instance;
        if (_gameManager != null)
            _gameManager.bullets.Add(this);
        StartCoroutine(BulletRoutine());
    }

    private IEnumerator BulletRoutine()
    {
        Vector3 startPos = transform.position;
        float scatterDistSqr = scatterDistance * scatterDistance;

        while (true)
        {
            float distSqr = (transform.position - startPos).sqrMagnitude;
            if (distSqr >= scatterDistSqr)
                break;

            float t = Mathf.Sqrt(distSqr) / scatterDistance;
            float speed = Mathf.Lerp(BulletSpeed, BulletSpeed * 0.1f, t);
            _rigidbody.MovePosition(_rigidbody.position + transform.forward * (speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        _rigidbody.linearVelocity = Vector3.zero;

        yield return new WaitForSeconds(pauseDuration);

        _inductive = true;
        _fixedUpdateActive = true;

        yield return new WaitForSeconds(inductiveTime);

        _inductive = false;
    }

    private void FixedUpdate()
    {
        if (!_fixedUpdateActive) return;

        if (_inductive && _gameManager)
            transform.LookAt(_gameManager.playerPosition);

        _rigidbody.MovePosition(_rigidbody.position + transform.forward * (BulletSpeed * Time.fixedDeltaTime));
        CheckMaxDistance();
    }

    private void OnDestroy()
    {
        if (_gameManager == null || _gameManager.bullets == null)
            return;
        _gameManager.bullets.Remove(this);
    }
}
