using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionableThirdPerson : Positionable
    {
        protected override void GroundCheck()
        {
            IsGrounded = Physics.CheckSphere(myTransform.position, radiusGroundCheck, groundLayer);
            SurfaceAngle = _getSurfaceAngle();
        }

        private float _getSurfaceAngle()
        {
            RaycastHit hit;

            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, radiusGroundCheck, groundLayer))
            {
                SurfaceType = hit.collider.tag;

                return Vector3.Angle(hit.normal, Vector3.up);
            }
            else
            {
                SurfaceType = "Air";
            }

            return 0f;
        }
    }
}