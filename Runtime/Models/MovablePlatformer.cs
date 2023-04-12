using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovablePlatformer : MovablePreset
    {
        private float _getSpeedSliding(float slope = 45.0f) => slope * 0.1f * Time.fixedDeltaTime;

        private PositionablePreset _positionable;
        private Rigidbody2D _rigidbody;

        private float _timerGrounded = 0;

        private new void Awake()
        {
            base.Awake();

            _positionable = GetComponent<PositionablePreset>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.freezeRotation = true;
        }

        public override void StartMovement()
        {
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.freezeRotation = true;
        }

        public override void StopMovement()
        {
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            float velocityCoefficient2D = 51.0f;
            float currentSpeed = _positionable.IsSliding ? _getSpeedSliding(_positionable.SurfaceSlope) : speed * getSpeedScale;
            Vector2 velocity = VectorAcceleration(_positionable.Project(direction).normalized * currentSpeed * velocityCoefficient2D);
            
            _rigidbody.gravityScale = Gravity;

            IsFall = _positionable.IsGrounded == false && _rigidbody.velocity.y <= 0;
            IsJump = (IsJump == true && _rigidbody.velocity.y <= 0) || _positionable.IsSliding == true ? false : IsJump;

            _timerGrounded = _positionable.IsGrounded ? _timerGrounded + Time.deltaTime : 0;

            if (_timerGrounded > 0.2f)
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

        public override void Jump(float force)
        {
            if (_positionable.IsSliding == false)
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);

                IsJump = true;
            }
        }

        private void movement(Vector2 velocity)
        {
            float gravity = IsFall || IsJump ? _rigidbody.velocity.y : velocity.y;
            _rigidbody.velocity = new Vector2(velocity.x, gravity);
        }
    }
}