using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Activator/Grounded Activator")]
    public class GroundedActivator : Activator
    {
        private Positionable _positionable;

        public override void OnEnableState()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _positionable = AddComponentInRoot<Positionable>();
        }

        public override void OnInactiveState()
        {
            TryActive(_positionable.IsGrounded);
        }
    }
}