using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - for speed control. </summary>
    [AddComponentMenu("Actormachine/Model/Movable")]
    public class Movable : Model
    {
        // Stats Fields
        [Range(1, 5)] public float WalkSpeed = 3.0f;
        [Range(1, 10)] public float RunSpeed = 5.0f;
        [Range(0, 2)] public float Gravity = 1f;
        [Range(0, 5)] public int JumpHeight = 2;
        [Range(0, 2)] public int ExtraJumps = 0;
        [Range(0, 1)] public float Levitation = 0.25f;

        // Calculation Fields
        public int JumpCounter = 0;
        public Vector3 Velocity = Vector3.zero;

        // Buffer Fields
        private Vector3 _lerpDirection = Vector3.zero;

        // Unity Components
        private Rigidbody _rigidbody;
        private SphereCollider _groundCollider;
        private PhysicMaterial _materialOnTheGround;
        private PhysicMaterial _materialInTheAir;

        // Return Value
        public Vector3 GetVelocity(Vector3 direction, float speed, float deltaTime)
        {
            _lerpDirection = Vector3.Lerp(_lerpDirection, direction, deltaTime);

            return new Vector3(_lerpDirection.x, direction.y, _lerpDirection.z) * speed;
        }

        public virtual void Enable()
        {
            // Get Resources
            _materialInTheAir = Resources.Load<PhysicMaterial>("Physic/Player In The Air");
            _materialOnTheGround = Resources.Load<PhysicMaterial>("Physic/Player On The Ground");

            // Add or Get comppnent in the Root
            _groundCollider = AddComponentInRoot<SphereCollider>();
            _rigidbody = AddComponentInRoot<Rigidbody>();
        }

        public virtual void Enter()
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

        public virtual void Horizontal(Vector3 direction, float speed, float rate)
        {
            Velocity = GetVelocity(direction, speed, Time.fixedDeltaTime * rate);

            _rigidbody.MovePosition(_rigidbody.position + Velocity * Time.fixedDeltaTime);
            _rigidbody.AddForce(Physics.gravity * Gravity, ForceMode.Acceleration);
        }

        public virtual void Force(Vector3 force)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        public virtual void Material(bool friction)
        {
            _groundCollider.material = friction == true ? _materialOnTheGround : _materialInTheAir;
        }

        public virtual void Exit()
        {
            // Set Collider Parementers
            _groundCollider.isTrigger = true;

            // Set Movement Parameters 
            _rigidbody.MovePosition(_rigidbody.position);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }
    }
}