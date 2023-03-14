using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class Animatorable : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        public void Play(string name, float speed, float fade = 0.2f)
        {
            if (_animator)
            {
                _animator.CrossFade(name, fade);
                _animator.SetFloat("Speed", speed);
            }
        }
    }
}