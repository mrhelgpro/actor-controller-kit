using UnityEngine;

namespace AssemblyActorCore
{
    public class Damagable : MonoBehaviour
    {
        private Collider _bodyCollider;
        private Actionable _actionable;

        public GameObject GetRootObject => _actionable.gameObject;

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

                gameObject.tag = _actionable.transform.tag;
                gameObject.layer = LayerMask.NameToLayer("Damagable");
            }
        }

        public void TakeAction(GameObject action) => _actionable.TryToActivate(action);
    }
}