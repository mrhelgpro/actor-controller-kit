using UnityEngine;

namespace AssemblyActorCore
{
    public class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(-1, 1)] public float Gravity = 1;
        public bool FreezHorizontalPlane = false;

        public float a = 5f;

        private EnumMode _mode;
        private Transform _mainTransform;

        private Rigidbody _rigidbody;
        private Rigidbody2D _rigidbody2D;

        private Vector3 prePos;
       
        private float _speedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getAcceleration => Acceleration > 0 ? Acceleration + 1 : 1;

        private void Start()
        {
            Actor actor = GetComponentInParent<Actor>();

            _mainTransform = actor.gameObject.transform;
            _mode = actor.Mode;

            switch (_mode)
            {
                case EnumMode.ThirdPerson:
                    _rigidbody = actor.gameObject.GetComponent<Rigidbody>();
                    break;
                case EnumMode.Platformer:
                    _rigidbody2D = actor.gameObject.GetComponent<Rigidbody2D>();
                    break;
            }
        }

        public void MoveToDirection(Vector3 direction, float speed)
        {
            switch (_mode)
            {
                case EnumMode.Free:
                    VelocityFree(direction, speed);
                    break;
                case EnumMode.ThirdPerson:
                    VelocityThirdPerson(direction, speed);
                    break;
                case EnumMode.Platformer:
                    VelocityPlatformer(direction, speed);
                    break;
            }

            float speedCal = ((_mainTransform.position - prePos) / Time.deltaTime).magnitude;
            Debug.Log("SPEED " + speedCal);
            prePos = _mainTransform.position;
        }

        public void MoveToPosition(Vector3 direction, float speed)
        {
            switch (_mode)
            {
                case EnumMode.ThirdPerson:
                    _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    break;
                case EnumMode.Platformer:
                    _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                    break;
            }

            direction.Normalize();
            _mainTransform.position += direction * speed * Time.fixedDeltaTime;

            float speedCal = ((_mainTransform.position - prePos) / Time.deltaTime).magnitude;
            Debug.Log("SPEED " + speedCal);
            prePos = _mainTransform.position;
        }

        private void VelocityFree(Vector3 direction, float speed)
        {
            direction.Normalize();

            _mainTransform.position += direction * speed * _speedScale;
        }

        private void VelocityThirdPerson(Vector3 direction, float speed)
        {
            direction.Normalize();

            _rigidbody.constraints = FreezHorizontalPlane ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.None;
            _rigidbody.freezeRotation = true;

            if (direction == Vector3.zero && Gravity == 0)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            else
            {
                _rigidbody.MovePosition(_rigidbody.position + direction * speed * _speedScale);
                _rigidbody.AddForce(Physics.gravity * Gravity, ForceMode.Acceleration);
            }
        }

<<<<<<< HEAD
        private void VelocityPlatformer(Vector3 direction, float speed)
=======
        private void MoveForPlatformer(Vector2 direction, float speed)
>>>>>>> main
        {
            direction.Normalize();

            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _rigidbody2D.freezeRotation = true;

            _rigidbody2D.gravityScale = Gravity;

            float acceleration2D = 50;

            float horizontal = direction.x * speed * _speedScale * acceleration2D;
            float vertical = _rigidbody2D.velocity.y;

            _rigidbody2D.velocity = new Vector2(horizontal, vertical);
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