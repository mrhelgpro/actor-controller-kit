using UnityEngine;

namespace AssemblyActorCore
{
    public class Damagable : MonoBehaviour
    {
        private Collider _bodyCollider;
        private Actionable _actionable;
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
                _actionable = GetComponentInParent<Actionable>();
                _healthable = GetComponentInParent<Healthable>();

                gameObject.tag = _healthable.transform.tag;
                gameObject.layer = LayerMask.NameToLayer("Damagable");
            }
        }

        public void TakeDamage(float value) => _healthable.TakeDamage(value);
        public void TakeAction(GameObject action) => _actionable.TryToActivate(action);
    }
}