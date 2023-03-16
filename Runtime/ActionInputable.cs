using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionInputable : ActionByTime
    {
        public Inputable myInputable;

        protected override void Initialization() { }

        public override void WaitLoop()
        {
            if(myInputable.IsEqual(inputable)) actionable.Activate(myGameObject);
        }

        public override void Exit() => movable.FreezAll();
    }
}