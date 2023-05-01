using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class Positionable2D : Positionable
    {
        private Collision2D _groundCollision;
        private CircleCollider2D _groundCollider;

        private PhysicsMaterial2D _materialOnTheGround;
        private PhysicsMaterial2D _materialInTheAir;

        private void Start()
        {
            _groundCollider = GetComponent<CircleCollider2D>();

            _materialInTheAir = Resources.Load<PhysicsMaterial2D>("Physic2D/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicsMaterial2D>("Physic2D/Player On The Ground");
        }

        protected override void GroundCheck()
        {
            bool isGroundedCollision = _groundCollision == null ? false : true;
            bool isGroundedPhysics = Physics2D.OverlapCircle(RootTransform.position, 0.2f, groundLayer);

            IsGrounded = IsGrounded == true ? isGroundedPhysics : isGroundedCollision && isGroundedPhysics;
        }

        protected override void SurfaceCheck()
        {
            SurfaceType = IsGrounded == true && _groundCollision != null ? _groundCollision.gameObject.tag : "None";
            SurfaceNormal = IsGrounded == true && _groundCollision != null ? _groundCollision.contacts[0].normal : Vector3.zero;
        }

        protected override void ObstacleCheck()
        {
            float length = 0.35f;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + 0.25f, RootTransform.position.z);
            Vector3 direction = RootTransform.TransformDirection(Vector3.forward);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, length, layerMask);

            IsObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
        }

        private void materialCheck() // REMOVE THIS
        {
            _groundCollider.sharedMaterial = IsGrounded && IsObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }

        private void OnCollisionStay2D(Collision2D collision) => _groundCollision = collision;
        private void OnCollisionExit2D(Collision2D collision) => _groundCollision = null;
    }
}