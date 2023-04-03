using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class PositionablePhysic : Positionable
    {
        private SphereCollider _groundCollider;
        private RaycastHit _hitGround;
        private RaycastHit _hitObstacle;

        private PhysicMaterial _materialOnTheGround;
        private PhysicMaterial _materialInTheAir;

        private new void Awake()
        {
            base.Awake();

            _groundCollider = GetComponent<SphereCollider>();

            _materialInTheAir = Resources.Load<PhysicMaterial>("Physic/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicMaterial>("Physic/Player On The Ground");
        }

        protected override void UpdatePosition()
        {
            groundCheck();  
            surfaceCheck();
            obstacleCheck();
            materialCheck();
        }

        private void groundCheck()
        {
            IsGrounded = Physics.CheckSphere(mainTransform.position, 0.2f, groundLayer);
        }

        private void surfaceCheck()
        {
            float length = 0.5f;
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

        private void materialCheck()
        {
            _groundCollider.material = IsGrounded && IsSliding == false && IsObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }
    }
}