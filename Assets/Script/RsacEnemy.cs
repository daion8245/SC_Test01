using System;
using System.Collections;
using UnityEngine;

public class RsacEnemy : Character, IEnemies
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject firePosition;
    [SerializeField] private float fireRate = 2;
    private bool _loop = true;

    private void Update()
    {
        if (_loop)
        {
            StartCoroutine(Fire());
        }
    }

    private void LateUpdate()
    {
        gameObject.transform.LookAt(GameManager.Instance.playerPosition);
    }

    private IEnumerator Fire()
    {
        _loop = false;
        yield return new WaitForSeconds(fireRate);
        FiringBullet();
        _loop = true;
    }

    public void FiringBullet()
    {
        Instantiate(prefab, firePosition.transform.position, transform.rotation);
    }
}