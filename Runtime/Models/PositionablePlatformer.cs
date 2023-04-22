using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class PositionablePlatformer : Positionable
    {
        private Collision2D _groundCollision;
        private CircleCollider2D _groundCollider;

        private PhysicsMaterial2D _materialOnTheGround;
        private PhysicsMaterial2D _materialInTheAir;

        private new void Awake()
        {
            base.Awake();

            _groundCollider = GetComponent<CircleCollider2D>();

            _materialInTheAir = Resources.Load<PhysicsMaterial2D>("Physic2D/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicsMaterial2D>("Physic2D/Player On The Ground");
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
            bool isGroundedCollision = _groundCollision == null ? false : true;
            bool isGroundedPhysics = Physics.CheckSphere(mainTransform.position, 0.2f, groundLayer);

            isGrounded = IsGrounded == true ? isGroundedPhysics : isGroundedCollision && isGroundedPhysics;
        }

        private void surfaceCheck()
        {
            surfaceType = IsGrounded == true && _groundCollision != null ? _groundCollision.gameObject.tag : "None";
            surfaceNormal = IsGrounded == true && _groundCollision != null ? _groundCollision.contacts[0].normal : Vector3.zero;
        }

        private void obstacleCheck()
        {
            float length = 0.35f;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + 0.25f, mainTransform.position.z);
            Vector3 direction = mainTransform.TransformDirection(Vector3.forward);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, length, layerMask);

            isObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
        }

        private void materialCheck()
        {
            _groundCollider.sharedMaterial = isGrounded && isObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }

        private void OnCollisionStay2D(Collision2D collision) => _groundCollision = collision;
        private void OnCollisionExit2D(Collision2D collision) => _groundCollision = null;
    }
}