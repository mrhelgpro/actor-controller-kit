using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionablePlatformer : Positionable
    {
        protected override void GroundCheck()
        {
            IsGrounded = Physics2D.OverlapCircle(myTransform.position, radiusGroundCheck, groundLayer);
            SurfaceAngle = _getSurfaceAngle();
        }

        private float _getSurfaceAngle()
        {
            RaycastHit2D hit = Physics2D.Raycast(myTransform.position, -Vector2.up, radiusGroundCheck, groundLayer);

            if (hit.collider != null)
            {
                SurfaceType = hit.collider.tag;

                return Vector3.Angle(hit.normal, Vector2.up);
            }
            else
            {
                SurfaceType = "Air";
            }

            return 0f;
        }
    }
}