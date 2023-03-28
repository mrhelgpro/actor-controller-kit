using UnityEngine;

namespace AssemblyActorCore
{
    public class Animatorable : MonoBehaviour
    {
        private Animator _animator;

        private void Start() => _animator = gameObject.GetComponentInChildren<Animator>();

        public void Play(string name, float speed, float fade = 0.2f)
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