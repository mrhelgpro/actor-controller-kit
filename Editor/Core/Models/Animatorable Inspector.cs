using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Animatorable))]
    [CanEditMultipleObjects]
    public class AnimatorableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Animatorable thisTarget = (Animatorable)target;
            Transform root = thisTarget.FindRootTransform;
            Animator animator = root.gameObject.GetComponentInChildren<Animator>();

            if (animator == null)
            {
                Inspector.DrawInfoBox("<ANIMATOR> - IS NOT FOUND", BoxStyle.Error);

                return;
            }

            if (Application.isPlaying)
            {
                Inspector.DrawInfoBox(animator.runtimeAnimatorController.name);
            }
            else
            {
                Inspector.DrawInfoBox("CONTROLS ANIMATIONS");
            }
        }
    }
}
