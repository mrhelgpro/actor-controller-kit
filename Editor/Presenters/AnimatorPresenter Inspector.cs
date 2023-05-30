using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(AnimatorProperty))]
    [CanEditMultipleObjects]
    public class AnimatorPropertyInspector : ActormachineBaseInspector
    {
        public override void OnInspectorGUI()
        {
            AnimatorProperty thisTarget = (AnimatorProperty)target;

            thisTarget.PlayName = EditorGUILayout.TextField("Time", thisTarget.PlayName);
            thisTarget.Controller = (RuntimeAnimatorController)EditorGUILayout.ObjectField("Controller", thisTarget.Controller, typeof(RuntimeAnimatorController), true);
            thisTarget.PlayMode = (PlayMode)EditorGUILayout.EnumPopup("Play Mode", thisTarget.PlayMode);

            if (thisTarget.PlayMode == PlayMode.ByTime)
            {
                thisTarget.Duration = EditorGUILayout.FloatField("Duration", thisTarget.Duration);
            }
        }
    }
}
