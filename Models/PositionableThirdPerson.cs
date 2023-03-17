using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionableThirdPerson : Positionable
    {
        private void Update()
        {
            IsGrounded = Physics.CheckSphere(transform.position, 0.25f, groundLayer);
        }
    }
}