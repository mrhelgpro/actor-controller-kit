using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(AnimatorPresenter))]
    public class AnimatorPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            AnimatorPresenter thisTarget = (AnimatorPresenter)target;

            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();

            if (thisTarget.AnimatorController == null)
            {
                DrawModelBox("You need to add an AnimatorController", BoxStyle.Error);
            }
        }
    }
}
