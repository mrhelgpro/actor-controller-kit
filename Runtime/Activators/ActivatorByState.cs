using UnityEngine;

namespace Actormachine
{
    public class ActivatorByState : Activator
    {
        public State State;

        public override void UpdateLoop()
        {
            if (State == null)
            {
                SetActive(true);

                return;
            }

            SetActive(State.IsActive == true);
        }
    }
}