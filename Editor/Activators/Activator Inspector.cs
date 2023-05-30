using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Activator))]
    [CanEditMultipleObjects]
    public class ActivatorInspector : ActormachineBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Activator thisTarget = (Activator)target;

            BoxStyle style = thisTarget.IsAvailable() ? BoxStyle.Active : BoxStyle.Default;
            
            Inspector.DrawSubtitle("ACTIVATES THE STATE", style);

            DrawBaseInspector();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(ActivatorByInput))]
    [CanEditMultipleObjects]
    public class ActivatorByInputInspector : ActivatorInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(ActivatorByInteraction))]
    [CanEditMultipleObjects]
    public class ActivatorByInteractionInspector : ActivatorInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}