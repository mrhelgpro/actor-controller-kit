using UnityEngine;

namespace AssemblyActorCore
{
    public class Animatorable : Model
    {
        private Animator _animator;

        private new void Awake()
        {
            base.Awake();

            _animator = RootTransform.GetComponentInChildren<Animator>();
        }

        public void Play(string name, float speed = 1.0f, float fade = 0.025f)
        {
            _animator?.CrossFade(name, fade);
            _animator?.SetFloat("Speed", speed);
        }

        public void SetFloat(string name, float value) => _animator?.SetFloat(name, value);
        public void SetFloat(string name, float value, float dampTime) => _animator?.SetFloat(name, value, dampTime, Time.deltaTime);
    }
}