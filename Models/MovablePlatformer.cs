using UnityEngine;

namespace AssemblyActorCore
{
    public class MovablePlatformer : Movable
    {
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

            _rigidbody.gravityScale = Gravity;

            float acceleration2D = 50;

            float horizontal = direction.x * speed * getSpeedScale * acceleration2D;
            float vertical = _rigidbody.velocity.y;

            _rigidbody.velocity = new Vector2(horizontal, vertical);
        }

        public override void Jump(float force)
        {

        }
    }
}