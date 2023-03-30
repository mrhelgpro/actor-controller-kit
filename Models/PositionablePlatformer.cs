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

        protected override void GroundCheck()
        {
            bool isGroundedCollision = _groundCollision == null ? false : true;
            bool isGroundedPhysics = Physics.CheckSphere(mainTransform.position, 0.2f, groundLayer);

            IsGrounded = IsGrounded == true ? isGroundedPhysics : isGroundedCollision && isGroundedPhysics;

            SurfaceType = IsGrounded == true && _groundCollision != null ? _groundCollision.gameObject.tag : "None";
            surfaceNormal = IsGrounded == true && _groundCollision != null ? _groundCollision.contacts[0].normal : Vector3.zero;

            _groundCollider.sharedMaterial = IsGrounded && IsNormalSlope ? _materialOnTheGround : _materialInTheAir;
        }

        private void OnCollisionStay2D(Collision2D collision) => _groundCollision = collision;
        private void OnCollisionExit2D(Collision2D collision) => _groundCollision = null;
    }
}