using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class PositionablePhysic : Positionable
    {
        private Collision _groundCollision;
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

        protected override void GroundCheck()
        {
            IsGrounded = _groundCollision == null ? false : true;
            SurfaceType = IsGrounded == true ? _groundCollision.gameObject.tag : "None";
            surfaceNormal = IsGrounded == true ? _groundCollision.contacts[0].normal : Vector3.Lerp(surfaceNormal, Vector3.zero, Time.fixedDeltaTime);

            _groundCollider.material = IsGrounded && IsNormalSlope ? _materialOnTheGround : _materialInTheAir;
        }

        private void OnCollisionStay(Collision collision) => _groundCollision = collision;
        private void OnCollisionExit(Collision collision) => _groundCollision = null;
    }
}