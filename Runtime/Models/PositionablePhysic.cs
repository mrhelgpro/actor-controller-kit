using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class PositionablePhysic : PositionablePreset
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

        public override void UpdateData()
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
            float length = 2.0f;
            RaycastHit hit;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + 0.25f, mainTransform.position.z);
            Physics.Raycast(origin, Vector3.down, out hit, length);

            SurfaceType = hit.collider != null ? hit.collider.tag : "None";
            SurfaceNormal = hit.collider != null ? hit.normal : Vector3.zero;
        }

        private void obstacleCheck()
        {
            float length = 0.35f;
            RaycastHit hit;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + 0.25f, mainTransform.position.z);
            Physics.Raycast(origin, mainTransform.TransformDirection(Vector3.forward), out hit, length);

            IsObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
        }

        private void materialCheck()
        {
            _groundCollider.material = IsGrounded && IsObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }
    }
}