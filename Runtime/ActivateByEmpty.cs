using UnityEngine;

namespace AssemblyActorCore
{
    public class ActivateByEmpty : Activator
    {
        public override void UpdateActivate()
        {
            if (actionable.IsEmpty)
            {
                TryToActivate();
            }
        }
    }
}
