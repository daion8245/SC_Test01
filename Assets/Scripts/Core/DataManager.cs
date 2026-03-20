using System;
using Parts;
using UnityEngine;

namespace Core
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;

        public int stage = 0;
        public int score = 0;
        public int gold = 0;
        public int killCount = 0;

        private PartsSlot[] _slots = new PartsSlot[2];

        public PartsSlot GetSlot(int index) => _slots[index];

        public void SetSlotType(int index, PartsType type)
        {
            _slots[index].type = type;
            _slots[index].slotID = index;
        }

        public void ResetSlots()
        {
            _slots = new PartsSlot[2];
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}