using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovablePlatformer : Movable
    {
        private Positionable _positionable;
        private Rigidbody2D _rigidbody;

        public bool _isFall = false;
        public bool _isJump = false;
        private float _timerGrounded = 0;

        private new void Awake()
        {
            base.Awake();

            _positionable = GetComponent<Positionable>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void SetMoving(bool state)
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

        public override void Horizontal(Vector3 direction, float speed, float rate, float gravity)
        {
            Vector3 velocity = GetVelocity(direction, rate) * speed * GetSpeedScale * 51.0f;

            _rigidbody.gravityScale = gravity;

            _isFall = _positionable.IsGrounded == false && _rigidbody.velocity.y <= 0;
            _isJump = _isJump == true && _rigidbody.velocity.y <= 0 ? false : _isJump;

            _timerGrounded = _positionable.IsGrounded ? _timerGrounded + Time.deltaTime : 0;

            Debug.DrawLine(mainTransform.position, mainTransform.position + velocity * 5, Color.green, 0, true);

            if (_timerGrounded > 0.1f)
            {
                movement(velocity);
            }
            else
            {
                if (_positionable.IsGrounded == true)
                {
                    _rigidbody.velocity = Vector2.zero;
                }
                else
                {
                    movement(velocity);
                }
            }
        }

        public override void Vertical(float force)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);

            _isJump = true;
        }

        private void movement(Vector2 velocity)
        {
            float gravity = _isFall || _isJump ? _rigidbody.velocity.y : velocity.y;
            _rigidbody.velocity = new Vector2(velocity.x, gravity);
        }
    }
}