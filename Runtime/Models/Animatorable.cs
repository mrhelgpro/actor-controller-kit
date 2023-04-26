using UnityEngine;

namespace AssemblyActorCore
{
    public class Animatorable : Model
    {
        private Animator _animator;
        private string _previousName = "None";

        public override void Initialization(Transform transform)
        {
            base.Initialization(transform);
            _animator = RootTransform.GetComponentInChildren<Animator>();
        }

        public void Play(string name, float fade = 0.025f)
        {
            if (name != _previousName)
            {
                _animator?.CrossFade(name, fade);
                _previousName = name;
            }
        }
        public void SetFloat(string name, float value) => _animator?.SetFloat(name, value);
        public void SetFloat(string name, float value, float dampTime) => _animator?.SetFloat(name, value, dampTime, Time.deltaTime);
    }
}