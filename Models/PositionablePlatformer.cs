using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionablePlatformer : Positionable
    {
        private void Update()
        {
            IsGrounded = Physics2D.OverlapCircle(myTransform.position, 0.1f, groundLayer);

            SurfaceAngle = _getSurfaceAngle();
        }

        private float _getSurfaceAngle2D()
        {
            RaycastHit2D hit;

            if (Physics2D.Raycast(myTransform.position, -Vector2.up, 1f))
            {
                hit = Physics2D.Raycast(myTransform.position, -Vector2.up, 1f);

                return Vector3.Angle(hit.normal, Vector3.up);
            }

            return 0f;
        }

        public float _getSurfaceAngle()
        {
            RaycastHit2D hit = Physics2D.Raycast(myTransform.position, -Vector2.up);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);

                return Mathf.Rad2Deg * Mathf.Atan2(hit.normal.x, hit.normal.y);
            }

            return 0f;
        }
    }
}