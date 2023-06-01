using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Activator/Input Activator")]
    public sealed class InputActivator : Activator
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