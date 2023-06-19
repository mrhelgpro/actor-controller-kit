using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(InputPlayerController))]
    [CanEditMultipleObjects]
    public class InputPlayerControllerInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            InputPlayerController thisTarget = (InputPlayerController)target;

            if (Inspector.CheckSingleInstanceOnScene<InputPlayerController>() == false) return;

            if (Application.isPlaying == false)
            {
                thisTarget.Player = EditorGUILayout.ObjectField("Player", thisTarget.Player, typeof(GameObject), true) as GameObject;
            }        

            if (thisTarget.Player == null)
            {
                thisTarget.Player = GameObject.FindGameObjectWithTag("Player");
            }

            if (thisTarget.Player == null)
            {
                Inspector.DrawSubtitle("<PLAYER> IS NOT FOUND", BoxStyle.Error);
                return;
            }

            if (Application.isPlaying == true)
            {
                // Draw in Play mode
                Inspector.DrawHeader(thisTarget.Player.name);
            }

            thisTarget.PointerSensitivityX = EditorGUILayout.Slider("Pointer Sensitivity X", thisTarget.PointerSensitivityX, 0.0f, 1f);
            thisTarget.PointerSensitivityY = EditorGUILayout.Slider("Pointer Sensitivity Y", thisTarget.PointerSensitivityY, 0.0f, 1f);

            // Show Enum Mode
            if (thisTarget.MoveDirectionMode == InputPlayerController.MoveMode.Input || thisTarget.MoveDirectionMode == InputPlayerController.MoveMode.Input2D)
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