using System;
using UnityEngine;

namespace AssemblyActorCore
{
    /// <summary> Model - to control the Animator. </summary>
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

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [UnityEditor.CustomEditor(typeof(Animatorable))]
    public class AnimatorableEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            Animatorable thisTarget = (Animatorable)target;
            Transform root = thisTarget.FindRootTransform;
            Animator animator = root.gameObject.GetComponentInChildren<Animator>();

            if (animator == null)
            {
                DrawModelBox("<Animator> - is not found", BoxStyle.Error);

                return;
            }

            if (Application.isPlaying)
            {
                DrawModelBox(animator.runtimeAnimatorController.name);
            }
            else
            {
                DrawModelBox("Controls animations");
            }
        }
    }
#endif
}