using UnityEngine;

namespace Parts
{
    /// <summary>
    /// 화면 상 모든 미사일을 바라보는 적에게 강제 유도
    /// </summary>
    public class ForcedGuidanceParts : PartsBase
    {
        protected override void OnUsePart()
        {
            EnemyBase nearEnemy = FindClosestEnemy(GameManager.Instance.playerPosition);
            if (nearEnemy == null) return;

            Vector3 nearEnemyPosition = nearEnemy.transform.position;
            foreach (var blt in GameManager.Instance.bullets.ToArray())
            {
                blt.LookPosition = nearEnemyPosition;
                blt.ForcedInduction();
            }
        }

        private EnemyBase FindClosestEnemy(Vector3 from)
        {
            EnemyBase closest = null;
            float minDist = Mathf.Infinity;

            foreach (EnemyBase enemy in GameManager.Instance.enemies)
            {
                float sqrDist = (enemy.transform.position - from).sqrMagnitude;
                if (sqrDist < minDist)
                {
                    minDist = sqrDist;
                    closest = enemy;
                }
            }

            return closest;
        }
    }
}
