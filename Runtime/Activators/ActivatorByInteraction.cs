using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public class ActivatorByInteraction : Activator, IEnableState, IInactiveState, IEnterState
    {   
        private List<Transform> _targets = new List<Transform>();
        private Transform _rootTransform;

        public bool IsExists(Transform target) => _targets.Exists(t => t == target);

        public void OnEnableState()
        {
            _rootTransform = FindRootTransform;
        }

        public void OnInactiveState()
        {
            TryActive(_targets.Count > 0);
        }

        public new void OnEnterState()
        {
            base.OnEnterState();

            Transform target = _targets[0]; // FIX IT !!! Get Nearest

            _targets.Remove(target);

            target.parent = ThisTransform.parent; // FIXED IT!!!!
            //target.localPosition = Vector3.zero;
            //target.localEulerAngles = Vector3.zero;

            State[] states = target.GetComponentsInChildren<State>();

            foreach (State state in states) state.OnEnableState();

            state.Deactivate();
        }

        private void OnTriggerEnter(Collider collider)
        {
            // Checking for existence in the list
            if (IsExists(collider.transform) == true) return;

            // Checking for existence IInteractable
            IInteractable[] interactables = collider.GetComponents<IInteractable>();

            if (interactables.Length == 0) return;

            // Checking the available IInteractable
            int amountOfAvailable = 0;

            foreach (IInteractable interactable in interactables) amountOfAvailable += interactable.IsAvailable(_rootTransform) ? 1 : 0;

            if (amountOfAvailable != interactables.Length) return;

            // If you have passed the checks, add to the list
            _targets.Add(collider.transform);
        }

        private void OnTriggerExit(Collider collider)
        {
            // Checking for existence in the list
            if (IsExists(collider.transform) == false) return;

            // If you have passed the checks, remove to the list
            _targets.Remove(collider.transform);
        }
    }
}