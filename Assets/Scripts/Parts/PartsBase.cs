using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

namespace Parts
{
    public abstract class PartsBase : MonoBehaviour
    {
        [SerializeField] protected Image image;
        [SerializeField] protected string partName;
        [SerializeField] protected string partDiscription;
        [SerializeField] protected float cooldown;
        [SerializeField] protected float maxCooldown;
        
        public virtual bool UsePart()
        {
            if (!CooldownCheck())
                return false;
            
            OnUsePart();
            return true;
        }

        protected virtual bool CooldownCheck()
        {
            if (cooldown > 0)
                return false;
            
            cooldown = Time.time + cooldown;
            return true;
        }

        protected abstract void OnUsePart();
    }
}