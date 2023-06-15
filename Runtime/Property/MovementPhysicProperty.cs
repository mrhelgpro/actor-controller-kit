using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Property/MovementPhysic Property")]
    public sealed class MovementPhysicProperty : Property
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
        //private int _jumpCounter = 0;
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        private bool _isLevitation = false;

        // Model Components
        private Inputable _inputable;
        private Animatorable _animatorable;
        private Movable _movable;
        private Positionable _positionable;

        // Unity Components
        private Rigidbody _rigidbody;
        private SphereCollider _groundCollider;
        private PhysicMaterial _materialOnTheGround;
        private PhysicMaterial _materialInTheAir;

        // Property Methods
        public override void OnEnableState()
        {
            // Get Resources
            _materialInTheAir = Resources.Load<PhysicMaterial>("Physic/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicMaterial>("Physic/Player On The Ground");

            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
            _animatorable = AddComponentInRoot<Animatorable>();
            _movable = AddComponentInRoot<Movable>();
            _positionable = AddComponentInRoot<Positionable>();
            _groundCollider = AddComponentInRoot<SphereCollider>();
            _rigidbody = AddComponentInRoot<Rigidbody>();
        }

        public override void OnEnterState()
        {
            // Set Collider Parementers
            _groundCollider.isTrigger = false;
            _groundCollider.radius = 0.1f;
            _groundCollider.center = new Vector3(0, _groundCollider.radius, 0);

            // Set Movement Parementers
            _rigidbody.mass = 1;
            _rigidbody.drag = 0;
            _rigidbody.angularDrag = 0.05f;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.freezeRotation = true;
            _rigidbody.velocity = Vector3.zero;
        }

        public override void OnFixedActiveState()
        {
            // Set Movement Parameters    
            float speed = _inputable.ShiftState ? RunScale * _movable.RunSpeed : WalkScale * _movable.WalkSpeed;

            _currentDirection = _positionable.GetDirection(_inputable.MoveVector);
            _currentVelocity = _movable.GetVelocity(_currentDirection, speed, Time.fixedDeltaTime * Rate);

            _rigidbody.MovePosition(_rigidbody.position + _currentVelocity * Time.fixedDeltaTime);
            _rigidbody.AddForce(Physics.gravity * _movable.Gravity, ForceMode.Acceleration);

            // Set Jump Parameters
            if (_currentForce.magnitude > 0)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(_currentForce, ForceMode.Impulse);
                _currentForce = Vector3.zero;
            }

            // Set Animation Parameters
            _animatorable.Speed = _currentVelocity.magnitude;
            _animatorable.Grounded = _positionable.IsGrounded;
        }

        public override void OnActiveState()
        {
            jumpLoop();

            _groundCollider.material = _positionable.IsGrounded && _positionable.IsObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }

        public override void OnExitState()
        {
            // Set Movement Parameters 
            _currentDirection = Vector3.zero;
            _currentForce = Vector3.zero;
            
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;

            setLevitation(false);
        }

        private void jumpLoop()
        {
            // Input Jump
            _isJumpPressed = _inputable.MotionState;
            _movable.JumpCounter(_positionable.IsGrounded);

            setLevitation(_isJumpPressed);

            if (_isJumpPressed == false)
            {
                if (_positionable.IsGrounded)
                {
                    _isJumpDone = false;
                }
                else
                {
                    
                    if (_movable.JumpCounter(_positionable.IsGrounded) > 0)
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

                    if (_positionable)
                    {
                        _movable.JumpCounter(_positionable.IsGrounded, true);
                    }

                    _isJumpDone = true;
                }
            }
        }

        private void setLevitation(bool state)
        {
            if (state == true)
            {
                if (_isLevitation == false)
                {
                    _movable.Gravity = _movable.Gravity - _movable.Levitation;
                    _isLevitation = true;
                }
            }
            else
            {
                if (_isLevitation == true)
                {
                    _movable.Gravity = _movable.Gravity + _movable.Levitation;
                    _isLevitation = false;
                }
            }
        }
    }
}