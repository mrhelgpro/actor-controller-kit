using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class PositionableNavigation : Positionable
    {
        public override void UpdateParametres()
        {
            groundCheck();
            surfaceCheck();
            obstacleCheck();
        }

        private void groundCheck()
        {
            IsGrounded = Physics.CheckSphere(RootTransform.position, 0.2f, groundLayer);
        }

        private void surfaceCheck()
        {
            float length = 0.45f;
            RaycastHit hit;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + 0.25f, RootTransform.position.z);
            Physics.Raycast(origin, Vector3.down, out hit, length);

            SurfaceType = hit.collider != null ? hit.collider.tag : "None";
            SurfaceNormal = hit.collider != null ? hit.normal : Vector3.zero;
        }

        private void obstacleCheck()
        {
            float length = 0.35f;
            RaycastHit hit;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + 0.25f, RootTransform.position.z);
            Physics.Raycast(origin, RootTransform.TransformDirection(Vector3.forward), out hit, length);

            IsObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
        }
    }
}