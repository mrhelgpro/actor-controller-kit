using UnityEngine;

namespace AssemblyActorCore
{
    public class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing;
        [Range(0, 10)] public float Acceleration;
        private float _getSpeed => _getAcceleration * _getSlowing * Time.fixedDeltaTime * 10;
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

        public void SetMovement(Vector3 velocity, float gravity = 1.0f)
        {
            switch (_mode)
            {
                case EnumMode.Free:
                    _mainTransform.Move(velocity, _getSpeed);
                    break;
                case EnumMode.ThirdPerson:
                    _rigidbody.Move(velocity, _getSpeed, gravity);
                    break;
                case EnumMode.Platformer:
                    _rigidbody2D.Move(velocity, _getSpeed, gravity);
                    break;
            }  
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