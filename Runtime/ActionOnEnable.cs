using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionOnEnable : ActionByTime
    {  
        private void OnEnable() => actionable.Activate(myGameObject);

        public override void WaitLoop() => myGameObject.SetActive(false);

        public override void Exit()
        {
            myGameObject.SetActive(false);
            movable.FreezAll();
        }
    }
}