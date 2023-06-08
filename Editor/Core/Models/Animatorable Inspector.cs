using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Animatorable))]
    [CanEditMultipleObjects]
    public class AnimatorableInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Animatorable thisTarget = (Animatorable)target;
            Transform root = thisTarget.FindRootTransform;
            Animator animator = root.gameObject.GetComponentInChildren<Animator>();

            if (animator == null)
            {
                Inspector.DrawSubtitle("<ANIMATOR> - IS NOT FOUND", BoxStyle.Error);

                return;
            }

            if (Application.isPlaying == true)
            {
                Inspector.DrawSubtitle(animator.runtimeAnimatorController.name);
            }
            else
            {
                Inspector.DrawSubtitle("CONTROLS ANIMATIONS");
            }
        }
    }
}
