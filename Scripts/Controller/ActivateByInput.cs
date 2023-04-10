using UnityEngine;

namespace AssemblyActorCore
{
    public class ActivateByInput : Activator
    {
        public Input ActivateInput;
        protected Input input => _inputable.Input;

        private Inputable _inputable;

        private new void Awake()
        {
            base.Awake();

            _inputable = GetComponentInParent<Inputable>();
        }

        public override void UpdateActivate()
        {
            if (ActivateInput.IsButtonPress(input)) TryToActivate();
        }
    }
}