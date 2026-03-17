using System.Collections;
using UnityEngine;

public class GuidedMissile : BulletBase
{
    [SerializeField] private float inductiveTime;

    private bool _inductive = true;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InductiveSwitching());
    }

    protected override void Move()
    {
        if (_inductive && GameManager)
            transform.LookAt(GameManager.playerPosition);

        Rb.MovePosition(Rb.position + transform.forward * (BulletSpeed * Time.fixedDeltaTime));
    }

    private IEnumerator InductiveSwitching()
    {
        _inductive = true;
        yield return new WaitForSeconds(inductiveTime);
        _inductive = false;
    }
}
