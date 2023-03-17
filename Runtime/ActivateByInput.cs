using UnityEngine;

namespace AssemblyActorCore
{
    public class ActivateByInput : Activator
    {
        public Input ActivateInput;

        public override void UpdateActivate()
        {
            if (ActivateInput.IsButtonPress(input)) TryToActivate();
        }
    }
}