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
            
            float currentSpeed = _isSliding ? getSpeedSliding : speed * getSpeedScale * 51;
            Vector2 velocity = GetDirection(_positionable.Project(direction).normalized * currentSpeed); // 51 = acceleration2D 
            
            _rigidbody.gravityScale = Gravity;

            IsFall = _isGrounded == false && _rigidbody.velocity.y <= 0;
            IsJump = (IsJump == true && _rigidbody.velocity.y <= 0) || _isSliding == true ? false : IsJump;

            _timerGrounded = _isGrounded ? _timerGrounded + Time.deltaTime : 0;
            float slowing = _timerGrounded > 0.2f ? 1 : _isGrounded == false ? 1 : 0.1f;

            float gravity = IsFall || IsJump ? _rigidbody.velocity.y : velocity.y;
            _rigidbody.velocity = new Vector2(velocity.x * slowing, gravity);

            Vector3 end = velocity;
            Debug.DrawLine(mainTransform.position, mainTransform.position + end * 50, Color.red, 0, false);
        }

        public override void Jump(float force)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);

            IsJump = true;
        }
    }
}