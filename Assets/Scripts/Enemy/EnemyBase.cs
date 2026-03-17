using System.Collections;
using UnityEngine;

public class EnemyBase : Character, IEnemies
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
        if (GameManager.Instance != null)
            GameManager.Instance.enemies.Add(this);
    }

    protected virtual void LateUpdate()
    {
        if (GameManager.Instance)
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

    public Transform Transform => transform;

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.enemies.Remove(this);
    }
}