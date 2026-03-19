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
    [SerializeField] protected float moveSpeed = 5f;

    private Vector3 _moveTarget;
    private bool _hasTarget;

    protected virtual void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.enemies.Add(this);

        _moveTarget = PickRandomTargetInView();
        _hasTarget = true;

        StartCoroutine(FireLoop());
    }

    protected virtual void Update()
    {
        if (!isAlive) return;

        if (!_hasTarget)
        {
            _moveTarget = PickRandomTargetInView();
            _hasTarget = true;
        }

        Vector3 current = transform.position;
        Vector3 target = _moveTarget;
        target.y = current.y;

        float dist = Vector3.Distance(current, target);
        if (dist <= 0.5f)
        {
            _moveTarget = PickRandomTargetInView();
        }
        else
        {
            transform.position = Vector3.MoveTowards(current, target, moveSpeed * Time.deltaTime);
        }
    }

    protected virtual Vector3 PickRandomTargetInView()
    {
        Camera cam = Camera.main;
        if (cam == null) return transform.position;

        float viewX = Random.Range(0.10f, 0.90f);
        float viewY = Random.Range(0.10f, 0.90f);

        Vector3 viewportPoint = new Vector3(viewX, viewY, cam.WorldToViewportPoint(transform.position).z);
        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPoint);
        worldPos.y = transform.position.y;

        return worldPos;
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
