using System.Collections;
using UnityEngine;

public class GuidedMissile : BulletBase
{
    [SerializeField] private float inductiveTime;

    private bool _inductive = true;
    private GameManager _gameManager;

    protected override void Start()
    {
        base.Start();
        _gameManager = GameManager.Instance;
        if (_gameManager != null)
            _gameManager.bullets.Add(this);
        StartCoroutine(InductiveSwitching());
    }

    private void FixedUpdate()
    {
        if (_inductive && _gameManager)
            transform.LookAt(_gameManager.playerPosition);

        _rigidbody.MovePosition(_rigidbody.position + transform.forward * (BulletSpeed * Time.fixedDeltaTime));
        CheckMaxDistance();
    }

    private void OnDestroy()
    {
        if (_gameManager != null)
            _gameManager.bullets.Remove(this);
    }

    private IEnumerator InductiveSwitching()
    {
        _inductive = true;
        yield return new WaitForSeconds(inductiveTime);
        _inductive = false;
    }
}
