using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Deactivator))]
    [CanEditMultipleObjects]
    public class DeactivatorInspector : ActormachineBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawSubtitle("DEACTIVATES THE STATE");

            DrawBaseInspector();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(DeactivatorByInput))]
    [CanEditMultipleObjects]
    public class DeactivatorByInputInspector : DeactivatorInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}