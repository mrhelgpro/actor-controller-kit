using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovablePlatformer : Movable
    {
        private bool _isGrounded => _positionable.IsGrounded;
        private bool _isSliding => _positionable.IsSliding;

        private Positionable _positionable;
        private Rigidbody2D _rigidbody;

        private float _timerGrounded = 0;

        private new void Awake()
        {
            base.Awake();

            _positionable = GetComponent<Positionable>();
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
            float currentSpeed = _isSliding ? getSpeedSliding(_positionable.SurfaceSlope) : speed * getSpeedScale;
            Vector2 velocity = GetDirection(_positionable.Project(direction).normalized * currentSpeed * velocityCoefficient2D);
            
            _rigidbody.gravityScale = Gravity;

            IsFall = _isGrounded == false && _rigidbody.velocity.y <= 0;
            IsJump = (IsJump == true && _rigidbody.velocity.y <= 0) || _isSliding == true ? false : IsJump;

            _timerGrounded = _isGrounded ? _timerGrounded + Time.deltaTime : 0;

            if (_timerGrounded > 0.2f)
            {
                movement(velocity);
            }
            else
            {
                if (_isGrounded == true)
                {
                    _rigidbody.velocity = Vector2.zero;
                }
                else
                {
                    movement(velocity);
                }
            }
        }

        private void movement(Vector2 velocity)
        {
            float gravity = IsFall || IsJump ? _rigidbody.velocity.y : velocity.y;
            _rigidbody.velocity = new Vector2(velocity.x, gravity);
        }

        public override void Jump(float force)
        {
            if (_isSliding == false)
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);

                IsJump = true;
            }
        }
    }
}