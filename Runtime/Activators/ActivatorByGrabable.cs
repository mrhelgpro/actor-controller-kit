using UnityEngine;

namespace Actormachine
{
    public class ActivatorByGrabable : Activator
    {
        // Model Components
        private Interactable _interactable;
        private int _countGrabbable;

        public override void Enable()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _interactable = AddComponentInRoot<Interactable>();
        }

        public override void UpdateLoop()
        {
            SetActive(_countGrabbable > 0);
        }

        private void OnTriggerEnter(Collider collider)
        {
            Grabbable grabbable = collider.GetComponent<Grabbable>();

            if (grabbable)
            {
                if (_interactable.IsExists(collider.transform) == false)
                {
                    _interactable.Add(collider.transform);
                    _countGrabbable++;
                }
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Grabbable grabbable = collider.GetComponent<Grabbable>();

            if (grabbable)
            {
                if (_interactable.IsExists(collider.transform))
                {
                    _interactable.Remove(collider.transform);
                    _countGrabbable--;
                }
            }
        }
    }
}