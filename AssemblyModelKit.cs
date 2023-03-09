using UnityEngine;

namespace AssemblyActorCore
{
    public class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(-1, 1)] public float Gravity = 1;
        private float _speedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getAcceleration => Acceleration > 0 ? Acceleration + 1 : 1;

        private EnumMode _mode;
        private Transform _mainTransform;

        private Rigidbody _rigidbody;
        private Rigidbody2D _rigidbody2D;

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

        public void MoveByVelocity(Vector3 direction, float speed)
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
        }

        public void MoveToPosition(Vector3 direction, float speed)
        {
            if (_mode == EnumMode.ThirdPerson)
            {
                _rigidbody.useGravity = false;
            }
            else if (_mode == EnumMode.Platformer)
            {
                _rigidbody2D.gravityScale = 0;
            }

            direction.Normalize();
            _mainTransform.position += direction * speed * Time.fixedDeltaTime;
        }

        private void VelocityFree(Vector3 direction, float speed)
        {
            direction.Normalize();

            _mainTransform.position += direction * speed * _speedScale;
        }

        private void VelocityThirdPerson(Vector3 direction, float speed)
        {
            direction.Normalize();

            _rigidbody.useGravity = false;
            _rigidbody.MovePosition(_rigidbody.position + direction * speed * _speedScale);
            _rigidbody.AddForce(Physics.gravity * Gravity, ForceMode.Acceleration);
        }

        private void VelocityPlatformer(Vector3 direction, float speed)
        {
            direction.Normalize();

            _rigidbody2D.gravityScale = Gravity;
            _rigidbody2D.velocity = direction * speed * _speedScale;
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