using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovablePhysic : MovablePreset
    {
        private PositionablePreset _positionable;
        private Rigidbody _rigidbody;

        private new void Awake()
        {
            base.Awake();

            _positionable = GetComponent<PositionablePreset>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void StartMovement()
        {
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.freezeRotation = true;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
        }

        public override void StopMovement()
        {
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.isKinematic = true;
        }

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            Vector3 velocity = VectorAcceleration(_positionable.ProjectOntoSurface(direction).normalized * speed * getSpeedScale);
           
            float gravityScale = Gravity;

            IsFall = _positionable.IsGrounded == false && _rigidbody.velocity.y <= 0;
            IsJump = IsJump == true && _rigidbody.velocity.y <= 0 ? false : IsJump;

            _rigidbody.MovePosition(_rigidbody.position + velocity);
            _rigidbody.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }

        public override void Jump(float force)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);

            IsJump = true;
        }
    }
}