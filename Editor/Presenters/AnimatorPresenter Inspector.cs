using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(AnimatorPresenter))]
    [CanEditMultipleObjects]
    public class AnimatorPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            AnimatorPresenter thisTarget = (AnimatorPresenter)target;

            if (thisTarget.AnimatorController == null)
            {
                Inspector.DrawModelBox("You need to add an AnimatorController", BoxStyle.Error);
            }

            base.OnInspectorGUI();

            //thisTarget.AnimatorController = EditorGUILayout.ObjectField("Switch to", thisTarget.AnimatorController, typeof(RuntimeAnimatorController), true) as RuntimeAnimatorController;
        }
    }
}
