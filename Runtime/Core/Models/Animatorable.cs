using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - to control the Animator. </summary>
    public class Animatorable : ActorBehaviour
    {
        private float _speed;
        private Vector3 _direction;
        private string _previousName = "None";

        private AnimationClip _enterClip;
        private string _enterName;

        private Animator _animator = null;
        //private AnimatorOverrideController _previousOverrideController = null;
        private RuntimeAnimatorController _previousAnimatorController = null;

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

        public void Enter(RuntimeAnimatorController controller)
        {
            if (controller == null)
            {
                return;
            }

            AnimatorClipInfo[] currentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
            if (currentClipInfo.Length > 0)
            {
                _enterClip = currentClipInfo[0].clip;
            }

            //_previousOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            //_previousOverrideController.name = _animator.runtimeAnimatorController.name;

            _previousAnimatorController = _animator.runtimeAnimatorController;

            AnimatorOverrideController enterOverrideController = new AnimatorOverrideController(controller);

            AnimationClip[] clips = controller.animationClips;

            foreach (AnimationClip clip in clips)
            {
                Debug.Log(clip.name);
                enterOverrideController[clip.name] = clip;
            }

            enterOverrideController["Enter"] = _enterClip;
            _animator.runtimeAnimatorController = enterOverrideController;
            
            //_animator.Play("Enter", 0, 0.75f);
            _animator.CrossFade("Move", 0.025f);

            _previousName = "Enter";
        }

        public void Exit()
        {
            if (_previousAnimatorController == null)
            {
                return;
            }

            AnimatorClipInfo[] currentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
            if (currentClipInfo.Length > 0)
            {
                _enterClip = currentClipInfo[0].clip;
            }

            AnimatorOverrideController enterOverrideController = new AnimatorOverrideController(_previousAnimatorController);

            AnimationClip[] clips = _previousAnimatorController.animationClips;

            foreach (AnimationClip clip in clips)
            {
                enterOverrideController[clip.name] = clip;
            }

            enterOverrideController["Enter"] = _enterClip;
            _animator.runtimeAnimatorController = enterOverrideController;

            //_animator.Play("Enter", 0, 0.75f);
            _animator.CrossFade("Move", 0.025f);

            _previousName = "Exit";
        }

        public void Play(string name, float fade = 0.025f)
        {
            if (_animator)
            {
                if (name != _previousName)
                {
                    Debug.Log(name);
                    _animator.CrossFade(name, fade);
                    _animator.speed = 1;
                    _previousName = name;
                }
            }
        }

        public void Stop()
        {
            if (_animator)
            {
                _animator.speed = 0;
                _animator.SetFloat("Speed", 0);
            }

            _previousName = "Stop";
        }
    }
}