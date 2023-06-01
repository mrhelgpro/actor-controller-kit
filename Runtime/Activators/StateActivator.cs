using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Activator/State Activator")]
    public class StateActivator : Activator
    {
        public State State;

        public override void OnInactiveState()
        {
            if (State == null)
            {
                TryActive(true);

                return;
            }

            TryActive(State.IsActive == true);
        }
    }
}