using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Activator))]
    [CanEditMultipleObjects]
    public class ActivatorInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Activator thisTarget = (Activator)target;

            if (Application.isPlaying == false)
            {
                Inspector.DrawInfoBox("ACTIVATES THE PRESENTER");
            }
            else
            {
                string info = thisTarget.IsAvailable ? "AVAILABLE" : "WAITING";
                BoxStyle style = thisTarget.IsAvailable ? BoxStyle.Active : BoxStyle.Default;
                Inspector.DrawInfoBox(info, style);
            }

            base.OnInspectorGUI();
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