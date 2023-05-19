using UnityEngine;

namespace Actormachine
{
    public sealed class ActivatorByInput : Activator
    {
        public InputableCompare InputableCompare;
        
        private Inputable _inputable;

        public override void Initiate()
        {
            _inputable = AddComponentInRoot<Inputable>();
        }

        public override void UpdateLoop()
        {
            SetAvailable(InputableCompare.IsEquals(_inputable));
        }
    }
}