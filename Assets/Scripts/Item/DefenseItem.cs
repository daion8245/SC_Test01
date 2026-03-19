using UnityEngine;

namespace Item
{
    public class DefenseItem: ItemBase
    {
        protected override void GetItem(Collider other)
        {
            Player player = GetComponent<Player>();
            
        }
    }
}