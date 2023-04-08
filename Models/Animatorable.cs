using UnityEngine;

namespace AssemblyActorCore
{
    public class Animatorable : Model
    {
        private Animator _animator;

        private new void Awake()
        {
            base.Awake();

            _animator = mainTransform.GetComponentInChildren<Animator>();
        }

        public void Play(string name, float speed = 1.0f, float fade = 0.2f)
        {
            _animator?.CrossFade(name, fade);
            _animator?.SetFloat("Speed", speed);
        }

        public void SetSpeed(float value) => _animator?.SetFloat("Speed", value);
        public void SetJump(bool value) => _animator?.SetBool("Jump", value);
        public void SetGrounded(bool value) => _animator?.SetBool("Grounded", value);
        public void SetFall(bool value) => _animator?.SetBool("Fall", value);
    }
}