using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Model/Positionable2D")]
    public sealed class Positionable2D : Positionable
    {
        private Collision2D _groundCollision;

        protected override void GroundCheck()
        {
            bool isGroundedCollision = _groundCollision == null ? false : true;
            bool isGroundedPhysics = Physics2D.OverlapCircle(RootTransform.position, radiusGroundCheck * 0.75f, groundLayer);

            IsGrounded = IsGrounded == true ? isGroundedPhysics : isGroundedCollision && isGroundedPhysics;
        }

        protected override void SurfaceCheck()
        {
            SurfaceType = IsGrounded == true && _groundCollision != null ? _groundCollision.gameObject.tag : "None";
            SurfaceNormal = IsGrounded == true && _groundCollision != null ? _groundCollision.contacts[0].normal : Vector3.zero;
        }

        protected override void ObstacleCheck()
        {
            float offsetHeight = 0.25f;
            float length = 0.25f;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + offsetHeight, RootTransform.position.z);
            Vector3 direction = RootTransform.TransformDirection(Vector3.forward);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, length, layerMask);

            IsObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
        }

        protected override void AbyssCheck()
        {
            float offsetHeight = 0.5f;
            float length = 3.125f + offsetHeight;
            float offsetForward = 0.35f;
            float edgeDistance = 0.5f + offsetHeight;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + offsetHeight, RootTransform.position.z) + (RootTransform.TransformDirection(Vector3.forward) * offsetForward);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.down, length, layerMask);

            IsAbyss = hit.collider == null;
            IsEdge = hit.collider == null ? false : IsGrounded ? hit.distance > edgeDistance : false;
        }

        private void OnCollisionStay2D(Collision2D collision) => _groundCollision = collision;
        private void OnCollisionExit2D(Collision2D collision) => _groundCollision = null;
    }
}