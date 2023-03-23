using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovableThirdPerson : Movable
    {
        private Rigidbody _rigidbody;
        private new void Awake()
        {
            base.Awake();

            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        public override void FreezAll()
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
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

        public override void MoveToDirection(Vector3 direction, float speed, bool isGrounded = true)
        {
            Vector3 velocity = direction.normalized * speed * getSpeedScale;

            float gravityScale = Gravity;

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            else
            {
                IsFall = isGrounded == false && _rigidbody.velocity.y <= 0;
                IsJump = IsFall ? false : IsJump;

                _rigidbody.MovePosition(_rigidbody.position + velocity);
                _rigidbody.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
            }

            Debug.DrawLine(mainTransform.position, mainTransform.position + velocity * 5, Color.green, 0, false);
        }

        public override void Jump(float force)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);

            IsJump = true;
        }
    }
}