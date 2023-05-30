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

            thisTarget.PlayName = EditorGUILayout.TextField("Play Name", thisTarget.PlayName);

            thisTarget.OverrideController = EditorGUILayout.ObjectField("Override Controller", thisTarget.OverrideController, typeof(AnimatorOverrideController), true) as AnimatorOverrideController;

            thisTarget.PlayMode = (PlayMode)EditorGUILayout.EnumPopup("Play Mode", thisTarget.PlayMode);

            if (thisTarget.PlayMode == PlayMode.ByTime)
            {
                thisTarget.Duration = EditorGUILayout.FloatField("Duration", thisTarget.Duration);
            }
        }
    }
}
