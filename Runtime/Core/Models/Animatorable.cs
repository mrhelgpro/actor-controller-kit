using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace Actormachine
{

    /// <summary> Model - to control the Animator. </summary>
    public class Animatorable : ModelBehaviour
    {
        // Parameters
        private float _speed = 0;
        private Vector3 _direction = Vector3.zero;
        private bool _grounded = false;
        private string _previousName = "None";

        private RuntimeAnimatorController _previousController;

        private Animator _animator = null;

        private new void Awake()
        {
            base.Awake();

            _animator = RootTransform.GetComponentInChildren<Animator>();
        }

        public float Speed
        {
            get => _speed;

            set
            {
                _speed = value;

                if (_animator)
                {
                    _animator.SetFloat("Speed", _speed);
                }
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

                if (_animator)
                {
                    _animator.SetBool("Grounded", _grounded);
                }
            }
        }

        public void Enter(RuntimeAnimatorController nextController)
        {
            if (nextController == null)
            {
                return;
            }

            _previousController = _animator.runtimeAnimatorController;
            _animator.runtimeAnimatorController = nextController;

            _previousName = "Enter";
        }

        public void Exit()
        {
            if (_previousController == null)
            {
                return;
            }

            _animator.runtimeAnimatorController = _previousController;
            
            _previousName = "Exit";
        }

        public void Play(string name, float speed = 1)
        {
            if (_animator)
            {
                if (name != _previousName)
                {
                    float previousLength = GetCurrentLength();
                    float nextLength = GetNextLength(name, speed);
                    float length = (previousLength + nextLength) / 2;
                    float fade = 0.1f / length * speed * speed;

                    // Debug.Log("Pre: " + _previousName + " - " + previousLength + ", Next: " + name + " - " + nextLength + ", Fade: " + fade + ", Speed: " + speed);

                    _animator.CrossFade(name, fade, 0, 0);
                    _animator.speed = speed;
                    _previousName = name;
                }
            }
        }

        public void Stop()
        {   
            _previousName = "Stop";
        }

        private float GetClipLength(AnimationClip clip) => clip.length;

        private float GetCurrentLength()
        {
            // Try get by current Clip Info
            AnimatorClipInfo[] currentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);

            if (currentClipInfo.Length > 0)
            {
                AnimationClip clip = currentClipInfo[0].clip;

                if (clip != null)
                {
                    float length = GetClipLength(clip);

                    return (length == 0 ? 1 : length) / _animator.speed;
                }
            }

            return 1;
        }

        private float GetNextLength(string name, float speed)
        {
            // Try get by name
            AnimationClip[] animationClips = _animator.runtimeAnimatorController.animationClips;
            
            foreach (AnimationClip clip in animationClips)
            {
                if (clip.name == name)
                {
                    return GetClipLength(clip) / speed;
                }
            }

            // Try to predict by Parameters
            if (speed == 1)
            {
                return Speed > 0.1f ? 1 : 8;
            }

            return 1 / speed;
        }
    }
}