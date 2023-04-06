using UnityEngine;

namespace AssemblyActorCore
{
    public class Damagable : MonoBehaviour
    {
        private Collider _bodyCollider;
        private Healthable _healthable;

        protected void Start()
        {
            _bodyCollider = GetComponent<Collider>();

            if (_bodyCollider == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _bodyCollider.isTrigger = true;
                _healthable = GetComponentInParent<Healthable>();

                gameObject.tag = _healthable.transform.tag;
                gameObject.layer = LayerMask.NameToLayer("Damagable");
            }
        }

        public void TakeDamage(float value) => _healthable.TakeDamage(value);
    }
}