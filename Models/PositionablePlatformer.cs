using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionablePlatformer : Positionable
    {
        protected Collision2D groundCollision;

        protected override void GroundCheck()
        {
            IsGrounded = groundCollision == null ? false : true;
            SurfaceType = IsGrounded == true ? groundCollision.gameObject.tag : "None";
            surfaceNormal = IsGrounded == true ? groundCollision.contacts[0].normal : Vector3.zero;
        }

        private void OnCollisionStay2D(Collision2D collision) => groundCollision = collision;
        private void OnCollisionExit2D(Collision2D collision) => groundCollision = null;
    }
}