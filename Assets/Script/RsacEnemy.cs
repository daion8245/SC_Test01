using System;
using System.Collections;
using UnityEngine;

public class RsacEnemy : Character, IEnemies
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected GameObject firePosition;
    [SerializeField] protected float fireRate = 2;
    protected bool loop = true;

    protected void Update()
    {
        if (loop)
        {
            StartCoroutine(Fire());
        }
    }

    private void Start()
    {
        GameManager.Instance.enemies.Add(this);
    }

    protected void LateUpdate()
    {
        gameObject.transform.LookAt(GameManager.Instance.playerPosition);
    }

    protected virtual IEnumerator Fire()
    {
        loop = false;
        yield return new WaitForSeconds(fireRate);
        FiringBullet();
        loop = true;
    }

    public void FiringBullet()
    {
        Instantiate(prefab, firePosition.transform.position, transform.rotation);
    }
}