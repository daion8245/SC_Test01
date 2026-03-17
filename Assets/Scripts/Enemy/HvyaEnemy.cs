using System.Collections;
using UnityEngine;

public class HvyaEnemy : EnemyBase
{
    [SerializeField] private int bulletCount = 10;
    [SerializeField] private float spreadAngle = 60f;

    protected override IEnumerator Fire()
    {
        loop = false;
        yield return new WaitForSeconds(fireRate);

        Vector3 toPlayer = (GameManager.Instance.playerPosition - firePosition.transform.position).normalized;
        float baseAngle = Mathf.Atan2(toPlayer.x, toPlayer.z) * Mathf.Rad2Deg;
        float halfSpread = spreadAngle / 2f;
        float step = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = baseAngle - halfSpread + step * i;
            Quaternion rot = Quaternion.Euler(0f, angle, 0f);
            Instantiate(prefab, firePosition.transform.position, rot);
        }

        loop = true;
    }
}
