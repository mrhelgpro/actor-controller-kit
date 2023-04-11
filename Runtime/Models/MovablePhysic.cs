using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovablePhysic : Movable
    {
        private bool _isGrounded =>_positionable.IsGrounded;
        private bool _isSliding => _positionable.IsSliding;

        private Positionable _positionable;
        private Rigidbody _rigidbody;

        private float _timerGrounded = 0;

        private new void Awake()
        {
            base.Awake();

            _positionable = GetComponent<Positionable>();
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        public override void FreezAll()
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.zero;
        }

        public override void FreezRotation()
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.freezeRotation = true;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.zero;
        }

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            Vector3 velocity = GetDirection(_positionable.Project(direction).normalized * speed * getSpeedScale);
           
            float gravityScale = Gravity;

            IsFall = _isGrounded == false && _rigidbody.velocity.y <= 0;
            IsJump = IsJump == true && _rigidbody.velocity.y <= 0 ? false : IsJump;

            _timerGrounded = _isGrounded ? _timerGrounded + Time.deltaTime : 0;
            float grounding = _timerGrounded > 0.2f ? 1 : _isGrounded == false ? 1 : 0.1f; // Slowing during grounding
            float slowing = _isSliding == false ? grounding : 0.25f;                       // Speed during grounding

            _rigidbody.MovePosition(_rigidbody.position + velocity * slowing);
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