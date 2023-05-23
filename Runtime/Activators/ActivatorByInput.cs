using UnityEngine;

namespace Actormachine
{
    public sealed class ActivatorByInput : Activator
    {
        public InputableCompare InputableCompare;
        
        private Inputable _inputable;

        public override void Enable()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _inputable = AddComponentInRoot<Inputable>();
        }

        public override void UpdateLoop()
        {
            SetActive(InputableCompare.IsEquals(_inputable));
        }
    }
}