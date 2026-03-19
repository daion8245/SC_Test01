using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Parts
{
    public enum PartsType
    {
        None,
        ForcedGuidanceParts,
        SlowFieldParts,
    }

    internal struct PartsSlot
    {
        public PartsType type;
        public Image image;
        public int slotID;
        public PartsBase partsBase;
    }
    
    public class PartsManager : MonoBehaviour
    {
        [SerializeField] private GameObject quickSlot1;
        [SerializeField] private GameObject quickSlot2;
        
        private PartsSlot _slot1;
        private PartsSlot _slot2;
            
        private void Start()
        {
           _slot1.image = quickSlot1.GetComponentInChildren<Image>();
           _slot2.image = quickSlot2.GetComponentInChildren<Image>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (_slot1.type == PartsType.None)
                    return;
                
                bool result = _slot1.partsBase.UsePart();

                if (!result)
                    Debug.Log("아이템이 쿨타임 중입니다.");
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_slot2.type == PartsType.None)
                    return;

                bool result = _slot2.partsBase.UsePart();

                if (!result)
                    Debug.Log("아이템이 쿨타임 중입니다.");
            }
        }
    }
}