using System.Collections;
using UnityEngine;

namespace Item
{
    public class DefenseItem : ItemBase
    {
        [SerializeField] private int defenseBoost = 5;
        [SerializeField] private float duration = 5f;

        protected override void GetItem(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player == null) return;
            StartCoroutine(DefenseBoost(player));
        }

        private IEnumerator DefenseBoost(Player player)
        {
            player.Def += defenseBoost;
            yield return new WaitForSeconds(duration);
            player.Def -= defenseBoost;
        }
    }
}
