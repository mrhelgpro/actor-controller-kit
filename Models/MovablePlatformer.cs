using UnityEngine;

namespace AssemblyActorCore
{
    public class MovablePlatformer : Movable
    {
        protected const float acceleration2D = 51;
        private Rigidbody2D _rigidbody;

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

            _rigidbody.gravityScale = Gravity;

            IsFall = isGrounded == false && _rigidbody.velocity.y < 0;
            IsJump = IsFall ? false : IsJump;

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                float horizontal = direction.x * speed * getSpeedScale * acceleration2D;
                float vertical = direction.y * speed * getSpeedScale * acceleration2D;             

                float gravity = isGrounded == false || IsFall || IsJump ? _rigidbody.velocity.y : vertical;

                _rigidbody.velocity = new Vector2(horizontal, gravity);
            }
        }

        public override void Jump(float force)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);
            IsJump = true;
        }
    }
}