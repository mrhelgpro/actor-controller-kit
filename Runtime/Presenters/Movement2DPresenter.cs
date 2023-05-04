using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class Movement2DPresenter : Presenter
    {
        [Range(1, 5)] public float MoveSpeed = 3f;
        [Range(1, 10)] public float MoveShift = 5f;
        [Range(1, 10)] public int Rate = 10;
        [Range(0, 5)] public int JumpHeight = 2;
        [Range(0, 2)] public int ExtraJumps = 0;
        [Range(0, 1)] public float Levitation = 0f;
        [Range(0, 2)] public float Gravity = 1f;

        // Move Fields
        private Vector3 _currentDirection = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _currentForce = Vector3.zero;
        private Vector3 _lerpDirection = Vector3.zero;
        private float _currentSpeed = 0;
        private float _currentGravity = 1;

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

        protected override void Initiation()
        {
            // Get components using "GetComponentInRoot" to create them on <Actor>
            _inputable = GetComponentInRoot<Inputable>();
            _animatorable = GetComponentInRoot<Animatorable>();
            _movable = GetComponentInRoot<Movable>();
            _positionable = GetComponentInRoot<Positionable2D>();

            _groundCollider = GetComponentInRoot<CircleCollider2D>();
            _groundCollider.isTrigger = false;
            _groundCollider.radius = 0.25f;
            _groundCollider.offset = new Vector2(0, _groundCollider.radius);

            _rigidbody = GetComponentInRoot<Rigidbody2D>();
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;

            _materialInTheAir = Resources.Load<PhysicsMaterial2D>("Physic2D/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicsMaterial2D>("Physic2D/Player On The Ground");
        }

        public override void Enter()
        {
            _groundCollider.isTrigger = false;
            _groundCollider.radius = 0.25f;
            _groundCollider.offset = new Vector2(0, _groundCollider.radius);

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

        public override void UpdateLoop()
        {
            float maxSpeed = _inputable.ShiftState ? MoveShift : MoveSpeed;

            _currentSpeed = _movable.GetSpeed(maxSpeed);
            _currentGravity = _movable.GetGravity(Gravity);
            _currentDirection = _positionable.GetDirection(_inputable.MoveVector);

            _lerpDirection = Vector3.Lerp(_lerpDirection, _currentDirection, Time.deltaTime * Rate);
            _currentVelocity = new Vector3(_lerpDirection.x, _currentDirection.y, _lerpDirection.z) * _currentSpeed;

            _animatorable.Play(_positionable.IsGrounded ? Name : "Fall");
            _animatorable.SetFloat("Speed", _currentVelocity.magnitude);

            jumpLoop();
            materialLoop();
        }

        public override void Exit()
        {
            _currentVelocity = Vector3.zero;

            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            _rigidbody.gravityScale = _currentGravity;
            _rigidbody.velocity = new Vector2(_currentVelocity.x * 51.0f * Time.fixedDeltaTime, _rigidbody.velocity.y);

            if (_currentForce.magnitude > 0)
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(_currentForce, ForceMode2D.Impulse);

                _currentForce = Vector3.zero;
            }
        }

        private void jumpLoop()
        {
            // Input Jump
            _isJumpPressed = _inputable.MotionState;

            if (_isJumpPressed == false)
            {
                if (_isLevitationPressed == true)
                {
                    Gravity = Gravity + Levitation;

                    _isLevitationPressed = false;
                }

                if (_positionable.IsGrounded)
                {
                    _isJumpDone = false;
                    _jumpCounter = ExtraJumps;
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
                    _currentForce = Vector3.up * JumpHeight.HeightToForce(Gravity);

                    Gravity = Gravity - Levitation;

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

        private void materialLoop()
        {
            _groundCollider.sharedMaterial = _positionable.IsGrounded && _positionable.IsObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }
    }
}