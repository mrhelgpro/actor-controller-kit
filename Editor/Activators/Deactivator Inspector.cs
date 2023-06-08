using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Deactivator))]
    [CanEditMultipleObjects]
    public class DeactivatorInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI() => DrawBaseInspector();
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(InputDeactivator))]
    [CanEditMultipleObjects]
    public class InputDeactivatorInspector : DeactivatorInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(TimerDeactivator))]
    [CanEditMultipleObjects]
    public class TimerDeactivatorInspector : DeactivatorInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}