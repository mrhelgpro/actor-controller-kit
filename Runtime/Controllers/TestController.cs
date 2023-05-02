using UnityEngine;

namespace AssemblyActorCore
{
    public class TestController : Presenter
    {
        protected override void Initiation()
        {
            // Get components using "GetComponentInActor" to create them on <Actor>
        }

        public override void Enter()
        { 
        
        }

        public override void UpdateLoop()
        {
            Debug.Log("IS DOING");
        }

        public override void Exit()
        { 
        
        }
    }
}