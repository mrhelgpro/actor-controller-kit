using UnityEngine;
using UnityEditor;

namespace Actormachine
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(InputPlayerController))]
    public class InputPlayerControllerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            InputPlayerController thisTarget = (InputPlayerController)target;

            // Show Enum Mode
            if (thisTarget.MoveDirectionMode == InputPlayerController.MoveMode.Input)
            {
                thisTarget.MoveDirectionMode = (InputPlayerController.MoveMode)EditorGUILayout.EnumPopup("Move Mode", thisTarget.MoveDirectionMode);
            }
            else
            {
                thisTarget.MoveDirectionMode = (InputPlayerController.MoveMode)EditorGUILayout.EnumPopup("Move Mode", thisTarget.MoveDirectionMode);
                thisTarget.InputTargetMode = (InputPlayerController.TargetMode)EditorGUILayout.EnumPopup("Target Mode", thisTarget.InputTargetMode);
                thisTarget.TargetRequiredLayers = EditorGUILayout.MaskField("Target Required Layers", thisTarget.TargetRequiredLayers, UnityEditorInternal.InternalEditorUtility.layers);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(thisTarget);
                thisTarget.ClearTarget();
            }
        }
    }
}