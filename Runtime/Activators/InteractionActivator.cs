using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Activator/Interaction Activator")]
    public class InteractionActivator : Activator
    {   
        private List<Transform> _targets = new List<Transform>();

        public bool IsExists(Transform target) => _targets.Exists(t => t == target);

        public override void OnInactiveState()
        {
            TryActive(_targets.Count > 0);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            Transform target = _targets[0]; // FIX IT !!! Get Nearest

            _targets.Remove(target);

            target.parent = ThisTransform.parent; // FIXED IT!!!!

            State[] states = target.GetComponentsInChildren<State>();

            foreach (State state in states) actor.Add(state);

            actor.Deactivate(state);
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

            foreach (IInteractable interactable in interactables) amountOfAvailable += interactable.IsAvailable(RootTransform) ? 1 : 0;

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