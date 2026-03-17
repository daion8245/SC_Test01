using System.Collections;
using UnityEngine;

public abstract class EnemyBase : Character, IEnemies
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected GameObject firePosition;
    [SerializeField] protected float fireRate = 2;

    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.enemies.Add(this);
        StartCoroutine(FireLoop());
    }

    private IEnumerator FireLoop()
    {
        while (isAlive)
        {
            yield return Fire();
        }
    }

    protected virtual IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireRate);
        FiringBullet();
    }

    protected virtual void LateUpdate()
    {
        if (GameManager.Instance)
            gameObject.transform.LookAt(GameManager.Instance.playerPosition);
    }

    public void FiringBullet()
    {
        Instantiate(prefab, firePosition.transform.position, transform.rotation);
    }
}
