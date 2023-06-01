using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Property/Return To State")]
    public class ReturnToState : Property
    {
        public State State;
        private Actor _actor;

        public override void OnEnterState()
        {
            _actor = GetComponentInParent<Actor>();
        }

        public override void OnExitState()
        {
            if (State == null)
            {
                return;
            }

            _actor.Activate(State);
        }
    }
}