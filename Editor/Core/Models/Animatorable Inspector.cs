using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Animatorable))]
    public class AnimatorableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Animatorable thisTarget = (Animatorable)target;
            Transform root = thisTarget.FindRootTransform;
            Animator animator = root.gameObject.GetComponentInChildren<Animator>();

            if (animator == null)
            {
                Inspector.DrawModelBox("<Animator> - is not found", BoxStyle.Error);

                return;
            }

            if (Application.isPlaying)
            {
                Inspector.DrawModelBox(animator.runtimeAnimatorController.name);
            }
            else
            {
                Inspector.DrawModelBox("Controls animations");
            }
        }
    }
}
