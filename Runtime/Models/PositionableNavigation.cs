using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Model/PositionableNavigation")]
    public sealed class PositionableNavigation : Positionable
    {
        protected override void GroundCheck()
        {
            IsGrounded = Physics.CheckSphere(RootTransform.position, 0.25f, groundLayer);
        }
    }
}
