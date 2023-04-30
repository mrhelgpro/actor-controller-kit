using UnityEngine;

namespace AssemblyActorCore
{
    public class MovablePhysic : Movable
    {
        private Rigidbody _rigidbody;

        private new void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.freezeRotation = true;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
        }

        public override void SetForce(Vector3 force) 
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        protected override void Move()
        {
            _rigidbody.MovePosition(_rigidbody.position + Velocity * Time.fixedDeltaTime);
            _rigidbody.AddForce(Physics.gravity * MovementParametres.Gravity, ForceMode.Acceleration);
        }

        public override void Exit()
        {
            MovementParametres = new MovementParametres();
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector3.zero;
        }
    }
}