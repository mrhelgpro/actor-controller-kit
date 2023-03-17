using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionablePlatformer : Positionable
    {
        private void Update()
        {
            IsGrounded = Physics2D.OverlapCircle(transform.position, 0.25f, groundLayer); // LayerMask.NameToLayer("Default")
        }
    }
}