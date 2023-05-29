using UnityEngine;

namespace Actormachine
{
    public class ActivatorByState : Activator
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