using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class PositionableNavigation : Positionable
    {
        public override void UpdateData()
        {
            groundCheck();
            surfaceCheck();
            obstacleCheck();
        }

        private void groundCheck()
        {
            isGrounded = Physics.CheckSphere(mainTransform.position, 0.2f, groundLayer);
        }

        private void surfaceCheck()
        {
            float length = 0.45f;
            RaycastHit hit;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + 0.25f, mainTransform.position.z);
            Physics.Raycast(origin, Vector3.down, out hit, length);

            surfaceType = hit.collider != null ? hit.collider.tag : "None";
            surfaceNormal = hit.collider != null ? hit.normal : Vector3.zero;
        }

        private void obstacleCheck()
        {
            float length = 0.35f;
            RaycastHit hit;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + 0.25f, mainTransform.position.z);
            Physics.Raycast(origin, mainTransform.TransformDirection(Vector3.forward), out hit, length);

            isObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
        }
    }
}