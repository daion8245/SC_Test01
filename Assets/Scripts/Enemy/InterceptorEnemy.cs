using System.Collections;
using UnityEngine;

public class InterceptorEnemy : EnemyBase
{
    [SerializeField] private GameObject launchLine;

    private bool _looking = true;

    protected override IEnumerator Fire()
    {
        _looking = true;
        launchLine.SetActive(true);
        yield return new WaitForSeconds(fireRate / 2);
        launchLine.SetActive(false);
        _looking = false;
        yield return new WaitForSeconds(1f);
        FiringBullet();
        yield return new WaitForSeconds(fireRate / 2);
        _looking = true;
    }

    protected override void LateUpdate()
    {
        if (!GameManager.Instance)
            return;
        if (_looking)
            gameObject.transform.LookAt(GameManager.Instance.playerPosition);

        // 뷰포트 경계 클램핑
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
}
