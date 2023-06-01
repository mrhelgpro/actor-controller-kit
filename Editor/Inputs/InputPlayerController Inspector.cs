using UnityEngine;
using UnityEditor;

namespace Actormachine
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(InputPlayerController))]
    [CanEditMultipleObjects]
    public class InputPlayerControllerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            InputPlayerController thisTarget = (InputPlayerController)target;

            thisTarget.gameObject.tag = "Player";

            thisTarget.PointerSensitivityX = EditorGUILayout.Slider("Pointer Sensitivity X", thisTarget.PointerSensitivityX, 0.0f, 1f);
            thisTarget.PointerSensitivityY = EditorGUILayout.Slider("Pointer Sensitivity Y", thisTarget.PointerSensitivityY, 0.0f, 1f);

            // Show Enum Mode
            if (thisTarget.MoveDirectionMode == InputPlayerController.MoveMode.Input)
            {
                thisTarget.MoveDirectionMode = (InputPlayerController.MoveMode)EditorGUILayout.EnumPopup("Move Mode", thisTarget.MoveDirectionMode);
            }
            else
            {
                thisTarget.MoveDirectionMode = (InputPlayerController.MoveMode)EditorGUILayout.EnumPopup("Move Mode", thisTarget.MoveDirectionMode);
                thisTarget.InputTargetMode = (InputPlayerController.TargetMode)EditorGUILayout.EnumPopup("Target Mode", thisTarget.InputTargetMode);
                thisTarget.LayerMask = EditorGUILayout.MaskField("Layer Mask", thisTarget.LayerMask, UnityEditorInternal.InternalEditorUtility.layers);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(thisTarget);
                thisTarget.ClearTarget();
            }
        }
    }
}