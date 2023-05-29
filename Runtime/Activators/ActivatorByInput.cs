using UnityEngine;

namespace Actormachine
{
    public sealed class ActivatorByInput : Activator
    {
        public InputableCompare InputableCompare;
        
        private Inputable _inputable;

        public override void OnEnableState()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _inputable = AddComponentInRoot<Inputable>();
        }

        public override void OnInactiveState()
        {
            TryActive(InputableCompare.IsEquals(_inputable));
        }
    }
}