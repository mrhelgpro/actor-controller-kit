
using UnityEngine;

namespace AssemblyActorCore
{
    public class DelegateByTrigger : Delegator
    {
        private GameObject _action;
        public string TargetTag;

        private void Awake()
        {
            _action = transform.GetChild(0).gameObject;
        }

        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log("ENTER");

            TryToActivate(collider.GetComponent<Actionable>(), _action);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("ENTER");

            TryToActivate(collider.GetComponent<Actionable>(), _action);
        }
    }
}
