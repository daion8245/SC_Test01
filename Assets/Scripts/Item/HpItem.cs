using UnityEngine;

namespace Item
{
    public class HpItem : ItemBase
    {
        protected override void GetItem(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                return;
            
            player.Hp = player.MaxHp;
        }
    }
}