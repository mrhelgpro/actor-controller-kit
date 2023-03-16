using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class ActivateByInput : Activator
    {
        public Input ActivateInput;

        private void Update()
        {
            if (ActivateInput.IsButtonPress(input)) Activate();
        }

        protected override void UpdateActivate()
        { 
        
        }
    }
}
