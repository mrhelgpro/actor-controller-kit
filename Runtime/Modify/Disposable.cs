using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public class Disposable : StateBehaviour, IEnableState, IInactiveState
    {
        private List<State> _childStates = new List<State>();

        public void OnEnableState()
        {
            _childStates = new List<State>();

            foreach (State state in GetComponentsInChildren<State>()) _childStates.Add(state);
        }
        public void OnInactiveState()
        {
            // Check if at least one child state is active
            foreach (State state in _childStates)
            {
                if (state.IsActive == true) return;
            }

            // If child states are not active
            foreach (State state in _childStates) state.OnDisableState();

            ThisTransform.parent = null;
        }
    }
}