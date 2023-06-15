using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Property/MovementConstant Property")]
    public class MovementConstantProperty : Property
    {
        public float Force = 1;

        // Move Fields
        private Vector3 _currentDirection = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;

        // Model Components
        private Inputable _inputable;
        private Movable _movable;
        private Positionable _positionable;

        // Unity Components
        private Rigidbody _rigidbody;
        private SphereCollider _groundCollider;
        private PhysicMaterial _materialOnTheGround;
        private PhysicMaterial _materialInTheAir;

        public override void OnEnableState()
        {
            // Get Resources
            _materialInTheAir = Resources.Load<PhysicMaterial>("Physic/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicMaterial>("Physic/Player On The Ground");

            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
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

            // Set Material Parementers
            _groundCollider.material = _materialInTheAir;

            // Set Value
            _currentDirection = _inputable.MoveVector.magnitude > 0 ? _positionable.GetDirection(_inputable.MoveVector).normalized : RootTransform.TransformDirection(Vector3.forward).normalized;
        }

        public override void OnFixedActiveState()
        {
            // Set Movement Parameters    
            _currentVelocity = _currentDirection * Force * Time.fixedDeltaTime * 100;

            _rigidbody.MovePosition(_rigidbody.position + _currentVelocity * Time.fixedDeltaTime);
            _rigidbody.AddForce(Physics.gravity * _movable.Gravity, ForceMode.Acceleration);
        }

        public override void OnExitState()
        {
            // Set Movement Parameters 
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;

            // Set Material Parementers
            _groundCollider.material = _materialOnTheGround;
        }
    }
}