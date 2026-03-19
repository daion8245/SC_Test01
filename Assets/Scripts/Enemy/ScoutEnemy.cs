using System.Collections;
using UnityEngine;

public class ScoutEnemy : EnemyBase
{
    [SerializeField] private float scoutFireRate = 1.5f;

    protected override IEnumerator Fire()
    {
        yield return new WaitForSeconds(scoutFireRate);
        FiringBullet();
    }
}
