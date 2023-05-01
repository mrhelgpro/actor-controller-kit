using System;
using UnityEngine;

namespace AssemblyActorCore
{
    [Serializable]
    public class Animatorable : ActorComponent
    {
        //public RuntimeAnimatorController AnimatorController;

        private Animator _animator;
        private string _previousName = "None";

        private new void Awake()
        {
            base.Awake();

            _animator = RootTransform.GetComponentInChildren<Animator>();
        }

        /*
        public void Enter(string name, float fade = 0.025f)
        {

            if (AnimatorController != _animator.runtimeAnimatorController)
            {
                _animator.StopPlayback();
                _animator.runtimeAnimatorController = AnimatorController;
            }

            _animator?.CrossFade(name, fade);
            _animator?.SetFloat("Speed", 1);
        }
        */

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