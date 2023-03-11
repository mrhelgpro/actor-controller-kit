using UnityEngine;

namespace AssemblyActorCore
{
    public class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(-1, 1)] public float Gravity = 1;
       
        public float GetSpeedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getAcceleration => Acceleration > 0 ? Acceleration + 1 : 1;
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