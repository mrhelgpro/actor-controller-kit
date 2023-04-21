using UnityEngine;

namespace AssemblyActorCore
{
    public class ActivateByInput : Activator
    {
        public Input ActivateInput;

        protected Inputable inputable;

        private new void Awake()
        {
            base.Awake();

            inputable = GetComponentInParent<Inputable>();
        }

        public override void UpdateActivate()
        {
            //if (ActivateInput.IsButtonPress(input)) TryToActivate();
        }
    }
}