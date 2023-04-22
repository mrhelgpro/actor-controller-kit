using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class PositionablePhysic : Positionable
    {
        private SphereCollider _groundCollider;
        private PhysicMaterial _materialOnTheGround;
        private PhysicMaterial _materialInTheAir;

        private new void Awake()
        {
            base.Awake();

            _groundCollider = GetComponent<SphereCollider>();

            _materialInTheAir = Resources.Load<PhysicMaterial>("Physic/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicMaterial>("Physic/Player On The Ground");
        }

        public override void UpdateParametres()
        {
            groundCheck();  
            surfaceCheck();
            obstacleCheck();
            materialCheck();
        }

        private void groundCheck()
        {
            isGrounded = Physics.CheckSphere(mainTransform.position, 0.2f, groundLayer);
        }

        private void surfaceCheck()
        {
            float length = 2.0f;
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

        private void materialCheck()
        {
            _groundCollider.material = isGrounded && isObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }
    }
}