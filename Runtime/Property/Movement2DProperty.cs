using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Property/Movement2D Property")]
    public sealed class Movement2DProperty : Property
    {
        [Range(0, 1)] public float WalkScale = 1.0f;
        [Range(0, 1)] public float RunScale = 1.0f;
        [Range(0, 1)] public float JumpScale = 1.0f;
        [Range(1, 10)] public int Rate = 10;

        // Move Fields
        private Vector3 _currentDirection = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _currentForce = Vector3.zero;

        // Jump Fields
        private int _jumpCounter = 0;
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        private bool _isLevitationPressed = false;

        // Model Components
        private Inputable _inputable;
        private Animatorable _animatorable;
        private Movable _movable;
        private Positionable _positionable;

        // Unity Components
        private Rigidbody2D _rigidbody;
        private CircleCollider2D _groundCollider;

        private PhysicsMaterial2D _materialOnTheGround;
        private PhysicsMaterial2D _materialInTheAir;

        // Property Methods
        public override void OnEnterState()
        {
            // Get Resources
            _materialInTheAir = Resources.Load<PhysicsMaterial2D>("Physic2D/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicsMaterial2D>("Physic2D/Player On The Ground");

            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
            _animatorable = AddComponentInRoot<Animatorable>();
            _movable = AddComponentInRoot<Movable>();
            _positionable = AddComponentInRoot<Positionable2D>();
            _groundCollider = AddComponentInRoot<CircleCollider2D>();
            _rigidbody = AddComponentInRoot<Rigidbody2D>();

            // Set Collider Parementers
            _groundCollider.isTrigger = false;
            _groundCollider.radius = 0.2f;
            _groundCollider.offset = new Vector2(0, _groundCollider.radius);

            // Set Movement Parementers
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.simulated = true;
            _rigidbody.useAutoMass = false;
            _rigidbody.mass = 1;
            _rigidbody.drag = 0;
            _rigidbody.angularDrag = 0.05f;
            _rigidbody.gravityScale = 1;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody.freezeRotation = true;
        }

        public override void OnActiveState()
        {
            jumpLoop();

            _groundCollider.sharedMaterial = _positionable.IsGrounded && _positionable.IsObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }

        public override void OnFixedActiveState()
        {
            // Set Movement Parameters    
            float speed = _inputable.ShiftState ? RunScale * _movable.RunSpeed : WalkScale * _movable.WalkSpeed;

            _currentDirection = _positionable.GetDirection(_inputable.MoveVector);

            _currentVelocity = _movable.GetVelocity(_currentDirection, speed, Time.fixedDeltaTime * Rate);

            _rigidbody.gravityScale = _movable.Gravity;
            _rigidbody.velocity = new Vector2(_currentVelocity.x * 51.0f * Time.fixedDeltaTime, _rigidbody.velocity.y);

            // Set Jump Parameters
            if (_currentForce.magnitude > 0)
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(_currentForce, ForceMode2D.Impulse);

                _currentForce = Vector3.zero;
            }

            // Set Animation Parameters
            _animatorable.Speed = _currentVelocity.magnitude;
            _animatorable.Grounded = _positionable.IsGrounded;
        }

        public override void OnExitState()
        {
            // Set Movement Parameters 
            _currentDirection = Vector3.zero;
            _currentForce = Vector3.zero;

            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.velocity = Vector3.zero;

            // Set Animation Parameters
            _animatorable.Grounded = true;
        }

        private void jumpLoop()
        {
            // Input Jump
            _isJumpPressed = _inputable.MotionState;

            if (_isJumpPressed == false)
            {
                if (_isLevitationPressed == true)
                {
                    //Gravity = Gravity + Levitation;
                    _movable.Gravity = _movable.Gravity + _movable.Levitation;

                    _isLevitationPressed = false;
                }

                if (_positionable.IsGrounded)
                {
                    _isJumpDone = false;
                    _jumpCounter = _movable.ExtraJumps;
                }
                else
                {
                    if (_jumpCounter > 0)
                    {
                        _isJumpDone = false;
                    }
                }
            }

            // Force Update
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    _currentForce = JumpScale * Vector3.up * _movable.JumpHeight.HeightToForce(_movable.Gravity);

                    //Gravity = Gravity - Levitation;
                    _movable.Gravity = _movable.Gravity - _movable.Levitation;

                    if (_positionable)
                    {
                        if (_positionable.IsGrounded == false)
                        {
                            _jumpCounter--;
                        }
                    }

                    _isJumpDone = true;
                    _isLevitationPressed = true;
                }
            }
        }
    }
}