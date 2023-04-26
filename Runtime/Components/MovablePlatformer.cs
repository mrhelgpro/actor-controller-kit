using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovablePlatformer : Movable
    {
        private Rigidbody2D _rigidbody;

        private new void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void Enable(bool state)
        {
            if (state == true)
            {
                _rigidbody.MovePosition(_rigidbody.position);
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.constraints = RigidbodyConstraints2D.None;
                _rigidbody.freezeRotation = true;
            }
            else
            {
                _rigidbody.MovePosition(_rigidbody.position);
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        public override void SetForce(Vector3 force) 
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        protected override void Move()
        {
            _rigidbody.gravityScale = gravity;
            _rigidbody.velocity = new Vector2(Velocity.x * 51.0f * Time.fixedDeltaTime, _rigidbody.velocity.y); //velocity *= 51.0f * Time.fixedDeltaTime;

            Debug.DrawLine(RootTransform.position, RootTransform.position + Velocity * 5, Color.green, 0, true);
        }
    }
}