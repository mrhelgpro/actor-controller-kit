using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Activator))]
    [CanEditMultipleObjects]
    public class ActivatorInspector : ActormachineComponentBaseInspector
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
    [CustomEditor(typeof(InputActivator))]
    [CanEditMultipleObjects]
    public class InputActivatorInspector : ActivatorInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(InteractionActivator))]
    [CanEditMultipleObjects]
    public class InteractionActivatorInspector : ActivatorInspector
    {
        private void OnEnable()
        {
            InteractionActivator thisTarget = (InteractionActivator)target;

            //Give object the "Ignore Raycast" layer
            thisTarget.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(StateActivator))]
    [CanEditMultipleObjects]
    public class StateActivatorInspector : ActivatorInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(GroundedActivator))]
    [CanEditMultipleObjects]
    public class GroundedActivatorInspector : ActivatorInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}