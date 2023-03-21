using UnityEngine;

namespace AssemblyActorCore
{
    public class MovablePlatformer : Movable
    {
        protected const float acceleration2D = 51;
        private Rigidbody2D _rigidbody;

        public float yVelocity;
        public float yDirection;

        public float Difference;

        public float difMax = 4;
        public float difMin = 4;

        private new void Awake()
        {
            base.Awake();

            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        public override void FreezAll() 
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public override void FreezRotation() 
        {
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.freezeRotation = true;
        }

        public override void MoveToDirection(Vector3 direction, float speed, bool isGrounded = false)
        {
            direction.Normalize();

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                _rigidbody.gravityScale = Gravity;

                yDirection = direction.y * speed * getSpeedScale * acceleration2D;
                yVelocity = _rigidbody.velocity.y;

                Difference = Mathf.Abs(yDirection - yVelocity);

                bool range = Difference > difMax;

                float horizontal = direction.x * speed * getSpeedScale * acceleration2D;
                float vertical = _rigidbody.velocity.y < 0 ? yVelocity : range ? yVelocity : yDirection;

                Vector2 velocity = new Vector2(horizontal, vertical);

                _rigidbody.velocity = velocity;
            }
        }

        public override void Jump(float force)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);
        }
    }
}