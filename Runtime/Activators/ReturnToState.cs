using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public class ReturnToState : Presenter
    {
        public State State;
        private Actor _actor;

        public override void Enter()
        {
            _actor = GetComponentInParent<Actor>();
        }

        public override void Exit()
        {
            if (State == null)
            {
                return;
            }

            _actor.Activate(State);
        }
    }
}