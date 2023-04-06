using UnityEngine;

namespace AssemblyActorCore
{
    public class Damagable : MonoBehaviour
    {
        private Collider _bodyCollider;
        private Healthable _healthable;
        private ActionDamage _action;
        private Transform _mainTransform;

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
                _mainTransform = _healthable.transform;
                _action = _mainTransform.GetComponentInChildren<ActionDamage>();

                gameObject.tag = _mainTransform.tag;
                gameObject.layer = LayerMask.NameToLayer("Damagable");
            }
        }

        public void TakeDamage(float value, string name = "Damage")
        {
            if (_healthable.IsLastDamage(value))
            {
                _action.TakeDeath();
            }
            else
            {
                _action.TakeDamage(value, name);
            }
        }
    }
}