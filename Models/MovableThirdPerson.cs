using UnityEngine;

namespace AssemblyActorCore
{
    public class MovableThirdPerson : Movable
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

        public override void MoveToDirection(Vector3 direction, float speed, bool isGrounded = false)
        {
            direction.Normalize();

            float gravityScale = Gravity;

            if (IsFall == true)
            {
                IsFall = isGrounded == true && _rigidbody.velocity.y >= 0 ? false : IsFall;
            }
            else
            {
                IsFall = isGrounded == false && _rigidbody.velocity.y < 0;
            }

            IsJump = IsFall || jumpTimeIsOver(isGrounded) ? false : IsJump;

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            else
            {
                _rigidbody.MovePosition(_rigidbody.position + direction * speed * getSpeedScale);
                _rigidbody.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
            }
        }

        public override void Jump(float force)
        {
            //float extraGravity = 0.6f * Gravity + 0.825f;

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);

            //_rigidbody.AddForce(Vector3.up * force * extraGravity, ForceMode.Impulse);

            IsJump = true;
            jumpTime = Time.time;
        }
    }
}