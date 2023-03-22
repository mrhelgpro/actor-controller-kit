using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionableThirdPerson : Positionable
    {
        protected Collision groundCollision;

        protected override void GroundCheck()
        {
            IsGrounded = groundCollision == null ? false : true;
            SurfaceType = IsGrounded == true ? groundCollision.gameObject.tag : "None";
            surfaceNormal = IsGrounded == true ? groundCollision.contacts[0].normal : Vector3.zero;
        }

        private void OnCollisionStay(Collision collision) => groundCollision = collision;
        private void OnCollisionExit(Collision collision) => groundCollision = null;
    }
}