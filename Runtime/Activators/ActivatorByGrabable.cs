using UnityEngine;

namespace Actormachine
{
    public class ActivatorByGrabable : Activator
    {
        // Model Components
        private Interactable _interactable;

        public override void Initiate()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _interactable = AddComponentInRoot<Interactable>();
        }

        public override void UpdateLoop()
        {

        }

        private void OnTriggerEnter(Collider collider)
        {
            Grabbable grabbable = collider.GetComponent<Grabbable>();

            if (grabbable)
            {
                _interactable.SetTarget(grabbable.transform);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (_interactable.Target.IsExists)
            {
                if (collider.transform == _interactable.Target.GetTransform)
                {
                    _interactable.Target.Clear();
                }
            }
        }
    }
}