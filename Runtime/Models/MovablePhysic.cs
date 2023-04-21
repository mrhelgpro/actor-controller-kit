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
        }

        public override void SetMoving(bool state)
        {
            if (state == true)
            {
                _rigidbody.MovePosition(_rigidbody.position);
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.constraints = RigidbodyConstraints.None;
                _rigidbody.freezeRotation = true;
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = false;
            }
            else
            {
                _rigidbody.MovePosition(_rigidbody.position);
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                _rigidbody.isKinematic = true;
            }
        }

        public override void Horizontal(Vector3 direction, float speed, float rate, float gravity)
        {
            Vector3 velocity = GetVelocity(direction, rate) * speed * GetSpeedScale;
            
            _rigidbody.MovePosition(_rigidbody.position + velocity);
            _rigidbody.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
        }

        public override void Vertical(float force)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}