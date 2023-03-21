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

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            direction.Normalize();

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                _rigidbody.gravityScale = Gravity;
                float horizontal = direction.x * speed * getSpeedScale * acceleration2D;
                float vertical =  _rigidbody.velocity.y;
                _rigidbody.velocity = new Vector2(horizontal, vertical);
            }
        }

        public override void Jump(float force)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);
        }
    }
}