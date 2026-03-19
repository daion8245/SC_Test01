using System.Collections;
using UnityEngine;

namespace Item
{
    public class InvincibleItem : ItemBase
    {
        [SerializeField] private float invincibleTime = 3f;
        
        protected override void GetItem(Collider other)
        {
            StartCoroutine(Invincible(other));
        }

        private IEnumerator Invincible(Collider other)
        {
            other.enabled = false;
            yield return new WaitForSeconds(invincibleTime);
            other.enabled = true;
        }
    }
}