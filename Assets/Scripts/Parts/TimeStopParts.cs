using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parts
{
    public class TimeStopParts : PartsBase
    {
        [SerializeField] private float stopDuration = 3f;

        protected override void OnUsePart()
        {
            StartCoroutine(TimeStop());
        }

        private IEnumerator TimeStop()
        {
            var bullets = GameManager.Instance.bullets.ToArray();
            Dictionary<IBullets, float> originalSpeeds = new Dictionary<IBullets, float>();

            foreach (var bullet in bullets)
            {
                originalSpeeds[bullet] = bullet.BulletSpeed;
                bullet.BulletSpeed = 0f;
            }

            yield return new WaitForSeconds(stopDuration);

            foreach (var kvp in originalSpeeds)
            {
                if (kvp.Key is MonoBehaviour mb && mb != null)
                    kvp.Key.BulletSpeed = kvp.Value;
            }
        }
    }
}
