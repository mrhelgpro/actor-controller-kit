using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionableFree : Positionable
    {
        private RaycastHit _hitGround;
        private RaycastHit _hitObstacle;

        protected override void UpdatePosition()
        {
            groundCheck();
            surfaceCheck();
            obstacleCheck();
        }

        private void groundCheck()
        {
            IsGrounded = Physics.CheckSphere(mainTransform.position, 0.2f, groundLayer);
        }

        private void surfaceCheck()
        {
            float length = 0.45f;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + 0.25f, mainTransform.position.z);
            Physics.Raycast(origin, Vector3.down, out _hitGround, length);

            SurfaceType = _hitGround.collider != null ? _hitGround.collider.tag : "None";
            surfaceNormal = _hitGround.collider != null ? _hitGround.normal : Vector3.zero;
        }

        private void obstacleCheck()
        {
            float length = 0.275f;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + 0.25f, mainTransform.position.z);
            Vector3 direction = mainTransform.TransformDirection(Vector3.forward);
            Physics.Raycast(origin, direction, out _hitObstacle, length);

            IsObstacle = _hitObstacle.collider == null ? false : _hitObstacle.collider.isTrigger ? false : true;
        }
    }
}
