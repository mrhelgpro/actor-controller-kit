using UnityEngine;

namespace Actormachine
{
    public sealed class MovementPhysicPresenter : Presenter
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
        private Rigidbody _rigidbody;
        private SphereCollider _groundCollider;
        private PhysicMaterial _materialOnTheGround;
        private PhysicMaterial _materialInTheAir;

        // Presenter Methods
        public override void Initiate() 
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _inputable = AddComponentInRoot<Inputable>();
            _animatorable = AddComponentInRoot<Animatorable>();
            _movable = AddComponentInRoot<Movable>();
            _positionable = AddComponentInRoot<Positionable>();

            _groundCollider = AddComponentInRoot<SphereCollider>();
            setParametersCollider();

            _rigidbody = AddComponentInRoot<Rigidbody>();
            setParametersRigidbody();

            _materialInTheAir = Resources.Load<PhysicMaterial>("Physic/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicMaterial>("Physic/Player On The Ground");
        }

        public override void Enter()
        {
            setParametersCollider();
            setParametersRigidbody();
        }

        public override void UpdateLoop()
        {
            setParametersMaterial();

            jumpLoop();
        }

        public override void FixedUpdateLoop()
        {
            float speed = _inputable.ShiftState ? MoveShift : MoveSpeed;

            _currentGravity = _movable.GetGravity(Gravity);
            _currentDirection = _positionable.GetDirection(_inputable.MoveVector);

            _currentVelocity = _movable.GetVelocity(_currentDirection, speed, Time.fixedDeltaTime * Rate);

            _animatorable.Play(_positionable.IsGrounded ? StateName : "Fall");
            _animatorable.Speed = _currentVelocity.magnitude;

            _rigidbody.MovePosition(_rigidbody.position + _currentVelocity * Time.fixedDeltaTime);
            _rigidbody.AddForce(Physics.gravity * _currentGravity, ForceMode.Acceleration);

            if (_currentForce.magnitude > 0)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(_currentForce, ForceMode.Impulse);
                _currentForce = Vector3.zero;
            }
        }

        public override void Exit()
        {
            _currentDirection = Vector3.zero;
            _currentForce = Vector3.zero;
            
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }

        // Movement Methods
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

        private void setParametersMaterial()
        {
            _groundCollider.material = _positionable.IsGrounded && _positionable.IsObstacle == false ? _materialOnTheGround : _materialInTheAir;
        }

        private void setParametersCollider()
        {
            _groundCollider.isTrigger = false;
            _groundCollider.radius = 0.2f;
            _groundCollider.center = new Vector3(0, _groundCollider.radius, 0);
        }

        private void setParametersRigidbody()
        {
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
    }
}