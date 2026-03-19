using System.Collections;
using Core;
using UnityEngine;

public abstract class EnemyBase : Character, IEnemies
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected GameObject firePosition;
    [SerializeField] protected float fireRate = 2;
    [SerializeField] protected GameObject[] itemPrefabs;
    [SerializeField] protected float dropRate = 0.3f;

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

        // 맵 밖 복귀
        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
            if (viewPos.x < 0.05f || viewPos.x > 0.95f || viewPos.y < 0.05f || viewPos.y > 0.95f)
            {
                viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
                viewPos.y = Mathf.Clamp(viewPos.y, 0.05f, 0.95f);
                Vector3 clampedWorld = cam.ViewportToWorldPoint(viewPos);
                clampedWorld.y = transform.position.y;
                transform.position = Vector3.Lerp(transform.position, clampedWorld, Time.deltaTime * 2f);
            }
        }
    }

    public void FiringBullet()
    {
        Instantiate(prefab, firePosition.transform.position, transform.rotation);
    }

    public override void Dead()
    {
        // 점수/돈 획득 + 킬 카운트
        if (DataManager.Instance != null)
        {
            DataManager.Instance.score += 100;
            DataManager.Instance.gold += 10;
            DataManager.Instance.killCount++;
        }

        // 아이템 드롭
        if (itemPrefabs != null && itemPrefabs.Length > 0 && Random.value < dropRate)
        {
            int idx = Random.Range(0, itemPrefabs.Length);
            Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
        }

        if (GameManager.Instance != null)
            GameManager.Instance.enemies.Remove(this);

        base.Dead();
    }
}
