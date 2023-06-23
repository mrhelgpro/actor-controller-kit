using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(PointerPlayerViewer))]
    [CanEditMultipleObjects]
    public class PointerPlayerViewerInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            PointerPlayerViewer thisTarget = (PointerPlayerViewer)target;

            if (thisTarget.PointerScreenPrefab == null)
            {
                thisTarget.PointerScreenPrefab = Resources.Load<GameObject>("Pointer/Pointer (Screen)");
            }

            if (thisTarget.PointerGroundPrefab == null)
            {
                thisTarget.PointerGroundPrefab = Resources.Load<GameObject>("Pointer/Pointer (Ground)");
            }

            if (thisTarget.PointerScopePrefab == null)
            {
                thisTarget.PointerScopePrefab = Resources.Load<GameObject>("Pointer/Pointer (Scope)");
            }

            if (Application.isPlaying == false)
            {
                DrawBaseInspector();

                return;
            }

            EditorGUILayout.LabelField("PointerScreenMode", PointerScreen.GetMode.ToString());
            EditorGUILayout.LabelField("PointerMovementMode", PointerMovement.GetMode.ToString());
            EditorGUILayout.LabelField("PointerScopeMode", PointerScope.GetMode.ToString());
        }
    }
}


