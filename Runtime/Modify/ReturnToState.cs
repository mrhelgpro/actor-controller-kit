using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public class ReturnToState : StateBehaviour, IEnterState, IExitState
    {
        public State State;
        private Actor _actor;

        public void OnEnterState()
        {
            _actor = GetComponentInParent<Actor>();
        }

        public void OnExitState()
        {
            if (State == null)
            {
                return;
            }

            _actor.Activate(State);
        }
    }
}