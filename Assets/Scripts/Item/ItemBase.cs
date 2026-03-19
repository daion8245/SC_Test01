using System;
using System.Collections;
using UnityEngine;

namespace Item
{
    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField] private float deletionTime = 8f;
        [SerializeField] private float hoveringSpeed = 1f;
        
        private Rigidbody _rb;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();
            StartCoroutine(ItemDelete());
        }

        protected IEnumerator ItemDelete()
        {
            yield return new WaitForSeconds(deletionTime);
            Destroy(this.gameObject);
        }

        private void Update()
        {
            _rb.linearVelocity = Vector3.down * (Mathf.Sin(Time.time * hoveringSpeed) * hoveringSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                GetItem(other);
                Destroy(gameObject);
            }
        }

        protected abstract void GetItem(Collider other);
    }
}
