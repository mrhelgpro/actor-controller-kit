using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionablePlatformer : Positionable
    {
        protected override void GroundCheck()
        {
            IsGrounded = Physics2D.OverlapCircle(mainTransform.position, radiusGroundCheck, groundLayer);
            SurfaceType = _getSurfaceType();
        }

        private string _getSurfaceType()
        {
            float height = 0.1f;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + height, mainTransform.position.z);

            RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, radiusGroundCheck + height, groundLayer);

            if (hit.collider != null)
            {
                return hit.collider.tag;
            }

            return "None";
        }

        private void OnCollisionEnter2D(Collision2D collision) => surfaceNormal = collision.contacts[0].normal;
    }
}