using System;
using System.Collections;
using UnityEngine;

public class InterceptorEnemy : RsacEnemy
{
    [SerializeField] private GameObject launchLine;
    
    private bool _looking = true;
    
    protected override IEnumerator Fire()
    {
        _looking = true;
        launchLine.SetActive(true);
        loop = false;
        yield return new WaitForSeconds(fireRate / 2);
        launchLine.SetActive(false);
        _looking = false;
        yield return new WaitForSeconds(1f);
        FiringBullet();
        yield return new WaitForSeconds(fireRate / 2);
        loop = true;
        _looking = true;
    }

    protected override void LateUpdate()
    {
        if (!GameManager.Instance)
            return;
        if (_looking)
            gameObject.transform.LookAt(GameManager.Instance.playerPosition);
    }
}
