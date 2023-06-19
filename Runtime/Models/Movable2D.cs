using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - for speed control. </summary>
    [AddComponentMenu("Actormachine/Model/Movable2D")]
    public class Movable2D : Movable
    {
        // Unity Components
        private Rigidbody2D _rigidbody2D;
        private CircleCollider2D _groundCollider2D;
        private PhysicsMaterial2D _materialOnTheGround2D;
        private PhysicsMaterial2D _materialInTheAir2D;

        public override void Enable()
        {
            // Get Resources
            _materialInTheAir2D = Resources.Load<PhysicsMaterial2D>("Physic2D/Player In The Air");
            _materialOnTheGround2D = Resources.Load<PhysicsMaterial2D>("Physic2D/Player On The Ground");

            // Add or Get comppnent in the Root
            _groundCollider2D = AddComponentInRoot<CircleCollider2D>();
            _rigidbody2D = AddComponentInRoot<Rigidbody2D>();
        }

        public override void Enter()
        {
            // Set Collider Parementers
            _groundCollider2D.isTrigger = false;
            _groundCollider2D.radius = 0.2f;
            _groundCollider2D.offset = new Vector2(0, _groundCollider2D.radius);

            // Set Movement Parementers
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.simulated = true;
            _rigidbody2D.useAutoMass = false;
            _rigidbody2D.mass = 1;
            _rigidbody2D.drag = 0;
            _rigidbody2D.angularDrag = 0.05f;
            _rigidbody2D.gravityScale = 1;
            _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody2D.freezeRotation = true;
        }

        public override void Horizontal(Vector3 direction, float speed, float rate)
        {
            Velocity = GetVelocity(new Vector3(direction.x, 0, 0), speed, Time.fixedDeltaTime * rate);

            _rigidbody2D.gravityScale = Gravity;
            _rigidbody2D.velocity = new Vector2(Velocity.x * 51.0f * Time.fixedDeltaTime, _rigidbody2D.velocity.y);
        }

        public override void Force(Vector3 force)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        }

        public override void Material(bool friction)
        {
            _groundCollider2D.sharedMaterial = friction == true ? _materialOnTheGround2D : _materialInTheAir2D;
        }

        public override void Exit()
        {
            // Set Collider Parementers
            _groundCollider2D.isTrigger = true;

            // Set Movement Parameters 
            _rigidbody2D.MovePosition(_rigidbody2D.position);
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _rigidbody2D.velocity = Vector3.zero;
        }
    }
}