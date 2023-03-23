using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovablePlatformer : Movable
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

        public override void MoveToDirection(Vector3 direction, float speed, bool  isGrounded = true)
        {
            Vector3 velocity = direction.normalized * speed * getSpeedScale * 51; // 51 = acceleration2D 

            _rigidbody.gravityScale = Gravity;

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                IsFall = isGrounded == false && _rigidbody.velocity.y <= 0;
                IsJump = IsFall ? false : IsJump;

                float gravity = IsFall || IsJump ? _rigidbody.velocity.y : velocity.y;
                _rigidbody.velocity = new Vector2(velocity.x, gravity);
            }

            Debug.DrawLine(mainTransform.position, mainTransform.position + velocity * 5, Color.green, 0, false);
        }

        public override void Jump(float force)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);

            IsJump = true;
        }
    }
}