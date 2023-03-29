using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovablePlatformer : Movable
    {
        private bool _isGrounded => _positionable.IsGrounded;
        private bool _iSlope => _positionable.IsNormalSlope == false;

        private Positionable _positionable;
        private Rigidbody2D _rigidbody;

        private new void Awake()
        {
            base.Awake();

            _positionable = GetComponent<Positionable>();
            _rigidbody = GetComponent<Rigidbody2D>();
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
            Vector3 velocity = GetDirection(_positionable.Project(direction).normalized * speed * getSpeedScale * 51); // 51 = acceleration2D 

            _rigidbody.gravityScale = Gravity;

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                IsFall = _isGrounded == false && _rigidbody.velocity.y <= 0;
                IsJump = IsJump == true && _rigidbody.velocity.y <= 0 ? false : IsJump;

                float gravity = IsFall || IsJump || _iSlope ? _rigidbody.velocity.y : velocity.y;
                _rigidbody.velocity = new Vector2(velocity.x, gravity);
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