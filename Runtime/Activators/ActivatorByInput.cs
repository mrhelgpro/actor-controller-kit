using UnityEngine;

namespace Actormachine
{
    public sealed class ActivatorByInput : Activator, IEnableState, IInactiveState
    {
        public InputableCompare InputableCompare;
        
        private Inputable _inputable;

        public void OnEnableState()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _inputable = AddComponentInRoot<Inputable>();
        }

        public void OnInactiveState()
        {
            TryActive(InputableCompare.IsEquals(_inputable));
        }
    }
}