using UnityEngine;

namespace AssemblyActorCore
{
    public class EdgeDetection : Action
    {
        public override void Enter() => movable.FreezAll();

        public override void UpdateLoop()
        {

        }

        public override void FixedLoop()
        {

        }

        public override void Exit() => movable.FreezAll();
    }
}