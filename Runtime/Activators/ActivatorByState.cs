using UnityEngine;

namespace Actormachine
{
    public class ActivatorByState : Activator, IInactiveState
    {
        public State State;

        public void OnInactiveState()
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