using UnityEngine;

namespace AssemblyActorCore
{
    public class MovableThirdPerson : Movable
    {
        private Rigidbody _rigidbody;

        public float Height;

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

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            direction.Normalize();

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            else
            {
                _rigidbody.MovePosition(_rigidbody.position + direction * speed * getSpeedScale);
                _rigidbody.AddForce(Physics.gravity * Gravity, ForceMode.Acceleration);
            }
        }

        /*
        A = 1 * (float)Mathf.Pow(4.528313f / Dependency, Force - 1);
        B = 1 * (float)Mathf.Pow(4.528313f / Dependency, A - 1);

        float x = 4.528313f / Force * A ;
        float k = Force * x;
        jumpForce = k / Force;
        */

        protected float getJumpForceScale(float force)
        {
            // if xForce = 2 then aForce = 3.181149f;
            // if xForce = 3 then aForce = 2.590051f

            // slope = (aForce2 - aForce1) / (xForce2 - xForce1) = (2.590051 - 3.181149) / (3 - 2) = -0.591098
            // intercept = aForce1 - slope * xForce1 = 3.181149 - (-0.591098 * 2) = 3.181149 + 1.182196 = 4.363345
            // aForce = slope * xForce + intercept

            float value = 1;

            switch (force)
            {
                case 1:
                    value = 4.528313f;
                    break;
            }

            jumpForce = value;

            return force * 3.181149f;
        }

        public override void Jump(float force)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}