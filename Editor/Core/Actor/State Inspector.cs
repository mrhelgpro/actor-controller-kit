using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(State))]
    [CanEditMultipleObjects]
    public sealed class StateInspector : ActorBehaviourInspector
    {
        private void OnEnable()
        {
            State thisTarget = (State)target;

            if (thisTarget)
            {
                // Checking for a single instance on object and destroy duplicates
                if (thisTarget.gameObject.CheckSingleInstanceOnObject<State>() == false) return;

                // Move Component To Up, if the Actor is at the other object
                if (thisTarget.GetComponent<Actor>() == null)
                {
                    ComponentUtility.MoveComponentUp(thisTarget);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            State thisTarget = (State)target;

            // Check Actor
            if (thisTarget.IsEnabled == false)
            {
                base.OnInspectorGUI();

                Inspector.DrawInfoBox("DISABLED", BoxStyle.Warning);
                
                return;
            }

            // Draw in Edit mode
            if (Application.isPlaying == false)
            {
                base.OnInspectorGUI();

                Inspector.DrawInfoBox("UPDATE THE PRESENTER, ACTIVATOR, DEACTIVATOR");
                
                return;
            }

            // Draw in Play mode
            string info = thisTarget.IsActive ? "ACTIVE" : "WAITING";
            BoxStyle style = thisTarget.IsActive ? BoxStyle.Active : BoxStyle.Default;

            Inspector.DrawHeader(thisTarget.Name);
            Inspector.DrawHeader(thisTarget.Priority.ToString(), 12);
            Inspector.DrawInfoBox(info, style);

            EditorUtility.SetDirty(thisTarget);
        }
    }
}