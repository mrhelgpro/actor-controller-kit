using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - to control the Animator. </summary>
    public class Animatorable : ActorBehaviour
    {
        private Animator _animator;
        private AnimatorOverrideController _previousOverrideController;
        private string _previousName = "None";

        private new void Awake()
        {
            base.Awake();

            _animator = RootTransform.GetComponentInChildren<Animator>();
        }

        public void Enter(RuntimeAnimatorController animatorController, string name, float fade = 0.025f)
        {
            if (animatorController == null)
            {
                return;
            }

            _previousOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = new AnimatorOverrideController(animatorController);
            //_animator.runtimeAnimatorController = animatorController;

            _animator.CrossFade(name, fade);
            _animator.SetFloat("Speed", 1);
        }

        public void Exit()
        {
            if (_previousOverrideController == null)
            {
                return;
            }

            _animator.runtimeAnimatorController = _previousOverrideController;
        }

        public void Play(string name, float fade = 0.025f)
        {
            if (_animator)
            {
                if (name != _previousName)
                {
                    _animator.CrossFade(name, fade);
                    _animator.speed = 1;
                    _previousName = name;
                }
            }
        }
        public void Speed(float value)
        {
            if (_animator)
            {
                _animator.speed = value;
            } 
        }
        public void Stop()
        {
            if (_animator)
            {
                _animator.StopPlayback();
                _animator.speed = 0;
                _animator.SetFloat("Speed", 0);
            }

            _previousName = "Stop";
        }
        public void SetFloat(string name, float value) => _animator?.SetFloat(name, value);
        public void SetFloat(string name, float value, float dampTime) => _animator?.SetFloat(name, value, dampTime, Time.deltaTime);
    }
}