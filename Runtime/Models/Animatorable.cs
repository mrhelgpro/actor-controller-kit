using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - to control the Animator. </summary>
    [AddComponentMenu("Actormachine/Model/Animatorable")]
    public class Animatorable : Model
    {
        private float _speed = 0;
        private Vector3 _direction = Vector3.zero;
        private bool _grounded = true;
        private float _rotation = 0;

        private AnimatorOverrideController _previousController;
        private Animator _animator = null;

        private new void Awake()
        {
            base.Awake();

            _animator = RootTransform.GetComponentInChildren<Animator>();
            _animator.speed = 0;
        }

        // Parameters
        public float Speed
        {
            get => _speed;

            set
            {
                _speed = value;

                _animator?.SetFloat("Speed", _speed);
            }
        }

        public Vector3 Direction
        {
            get => _direction;

            set
            {
                _direction = value;

                if (_animator)
                {
                    _animator.SetFloat("DirectionX", _direction.x);
                    _animator.SetFloat("DirectionY", _direction.y);
                    _animator.SetFloat("DirectionZ", _direction.z);
                }
            }
        }

        public bool Grounded
        {
            get => _grounded;

            set
            {
                _grounded = value;

                _animator?.SetBool("Grounded", _grounded);
            }
        }

        public float Rotation
        {
            get => _rotation;

            set
            {
                _rotation = value;

                _animator?.SetFloat("Rotation", _rotation);
            }
        }

        // Methods
        public void SetLayerWeight(int layer, float value)
        {
            _animator?.SetLayerWeight(layer, value);
        }

        public void Enter(AnimatorOverrideController nextController, float speed)
        {
            if (_animator)
            {
                if (nextController != null)
                {
                    _previousController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
                    _animator.runtimeAnimatorController = nextController;
                }

                _animator.speed = speed;

                _animator.Play("Full", 1, 0);
                _animator.Play("Top", 2, 0);
            }
        }

        public void Exit()
        {
            if (_previousController != null)
            {
                _animator.runtimeAnimatorController = _previousController;
            }

            if (_animator)
            {
                _animator.speed = 0;
            }
        }
    }
}