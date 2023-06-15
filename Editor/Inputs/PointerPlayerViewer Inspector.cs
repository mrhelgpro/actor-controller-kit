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
        public void OnEnable()
        {
            PointerPlayerViewer thisTarget = (PointerPlayerViewer)target;

            Canvas canvas = thisTarget.gameObject.AddRequiredComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            thisTarget.gameObject.AddRequiredComponent<CanvasScaler>();
            thisTarget.gameObject.AddRequiredComponent<GraphicRaycaster>();
        }

        public override void OnInspectorGUI()
        {
            PointerPlayerViewer thisTarget = (PointerPlayerViewer)target;

            Pointer.ScreenMode = (PointerScreenMode)EditorGUILayout.EnumPopup("Pointer Screen Mode", Pointer.ScreenMode);
            Pointer.GroundMode = (PointerGroundMode)EditorGUILayout.EnumPopup("Pointer Ground Mode", Pointer.GroundMode);
            Pointer.ScopeMode = (PointerScopeMode)EditorGUILayout.EnumPopup("Pointer Scope Mode", Pointer.ScopeMode);

            DrawBaseInspector();

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
        }
    }
}


