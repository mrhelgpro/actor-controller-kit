using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(AnimatorOverriderProperty))]
    [CanEditMultipleObjects]
    public class AnimatorOverriderPropertyInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            AnimatorOverriderProperty thisTarget = (AnimatorOverriderProperty)target;

            thisTarget.PlayMode = (PlayMode)EditorGUILayout.EnumPopup("Play Mode", thisTarget.PlayMode);
            thisTarget.LayerMode = (LayerMode)EditorGUILayout.EnumPopup("Layer Mode", thisTarget.LayerMode);

            if (thisTarget.LayerMode == LayerMode.Default)
            {
                thisTarget.TimeMode = TimeMode.Parameters;
            }
            else
            {
                thisTarget.TimeMode = (TimeMode)EditorGUILayout.EnumPopup("Time Mode", thisTarget.TimeMode);

                if (thisTarget.TimeMode == TimeMode.Timer)
                {
                    thisTarget.Duration = EditorGUILayout.FloatField("Duration", thisTarget.Duration);
                }
            }

            if (thisTarget.PlayMode == PlayMode.Override)
            {
                thisTarget.OverrideController = EditorGUILayout.ObjectField("Override Controller", thisTarget.OverrideController, typeof(AnimatorOverrideController), true) as AnimatorOverrideController;
            }
            else
            {
                thisTarget.OverrideController = null;
            }
        }
    }
}
