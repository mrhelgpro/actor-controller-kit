using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(-1, 1)] public float Gravity = 1;

        public enum Constraints
        { 
            None,
            HorizontalPlane,
            VerticalePlane,
            FreezAll
        }

        public float GetSpeedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getAcceleration => Acceleration > 0 ? Acceleration + 1 : 1;

        public abstract void MoveToDirection(Vector3 direction, float speed);
        public abstract void SetConstraints(Constraints constraints, bool freezeRotation = true);

        public void MoveToPosition(Vector3 direction, float speed)
        {
            direction.Normalize();

            transform.position += direction * speed * Time.fixedDeltaTime;
        }
    }

    public class MovableFree : Movable
    {
        private Transform _mainTransform;

        private void Awake() => _mainTransform = transform;

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            _mainTransform.position += direction * speed * GetSpeedScale;
        }

        public override void SetConstraints(Constraints constraints, bool freezeRotation = true)
        {

        }
    }

    public class MovableThirdPerson : Movable
    {
        private Rigidbody _rigidbody;

        private void Awake() => _rigidbody = gameObject.GetComponent<Rigidbody>();

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            direction.Normalize();

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            else
            {
                _rigidbody.MovePosition(_rigidbody.position + direction * speed * GetSpeedScale);
                _rigidbody.AddForce(Physics.gravity * Gravity, ForceMode.Acceleration);
            }
        }

        public override void SetConstraints(Constraints constraints, bool freezeRotation = true)
        {
            switch (constraints)
            {
                case Constraints.None:
                    _rigidbody.constraints = RigidbodyConstraints.None;
                    break;
                case Constraints.HorizontalPlane:
                    _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                    break;
                case Constraints.VerticalePlane:
                    _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
                    break;
                case Constraints.FreezAll:
                    _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    break;
            }

            _rigidbody.freezeRotation = freezeRotation;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.zero;
        }
    }

    public class MovablePlatformer : Movable
    {
        private Rigidbody2D _rigidbody;

        private void Awake() => _rigidbody = gameObject.GetComponent<Rigidbody2D>();

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            direction.Normalize();

            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.freezeRotation = true;

            _rigidbody.gravityScale = Gravity;

            float acceleration2D = 50;

            float horizontal = direction.x * speed * GetSpeedScale * acceleration2D;
            float vertical = _rigidbody.velocity.y;

            _rigidbody.velocity = new Vector2(horizontal, vertical);
        }

        public override void SetConstraints(Constraints constraints, bool freezeRotation = true)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.None;

            switch (constraints)
            {
                case Constraints.None:
                    _rigidbody.constraints = RigidbodyConstraints2D.None;
                    break;
                case Constraints.FreezAll:
                    _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    break;
            }

            _rigidbody.freezeRotation = freezeRotation;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector2.zero;
        }
    }

    public class Animatorable : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            Actor actor = GetComponentInParent<Actor>();
            _animator = actor.gameObject.GetComponentInChildren<Animator>();
        }

        public void SetAnimation(string name, float speed)
        {
            if (_animator)
            {
                _animator.CrossFade(name, 0.2f);
                _animator.SetFloat("Speed", speed);
            }
        }
    }
}