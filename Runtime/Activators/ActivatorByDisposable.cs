using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public class ActivatorByDisposable : Activator
    {
        private List<State> _childStates = new List<State>();

        private Collider _collider;
        private Rigidbody _rigidbody;

        public override void Enable()
        {
            _childStates = new List<State>();

            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            if (_collider) _collider.enabled = false;
            if (_rigidbody) _rigidbody.isKinematic = true;

            foreach (State state in GetComponentsInChildren<State>()) _childStates.Add(state);
        }
        public override void UpdateLoop()
        {
            SetActive(true);

            // Check if at least one child state is active
            foreach (State state in _childStates)
            {
                if (state.IsActive == true)
                {
                    return;
                }
            }

            // If child states are not active
            foreach (State state in _childStates) state.Disable();

            SetActive(false);

            ThisTransform.parent = null;

            if (_collider) _collider.enabled = true;
            if (_rigidbody) _rigidbody.isKinematic = false;
        }
    }
}