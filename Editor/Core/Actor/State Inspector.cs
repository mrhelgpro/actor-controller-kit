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

                // Add Actor in root transform
                thisTarget.AddComponentInRoot<Actor>();

                // Move Component To Up
                ComponentUtility.MoveComponentUp(thisTarget);
            }
        }

        public override void OnInspectorGUI()
        {
            State thisTarget = (State)target;

            // Draw in Edit mode
            if (Application.isPlaying == false)
            {
                base.OnInspectorGUI();
                
                Inspector.DrawInfoBox("UPDATE THE PRESENTER, ACTIVATOR, DEACTIVATOR");
                
                return;
            }

            // Draw in Play mode
            string info = thisTarget.IsCurrentState ? "ACTIVE" : "WAITING";
            BoxStyle style = thisTarget.IsCurrentState ? BoxStyle.Active : BoxStyle.Default;

            Inspector.DrawHeader(thisTarget.Name);
            Inspector.DrawHeader(thisTarget.Type.ToString(), 12);
            Inspector.DrawInfoBox(info, style);

            EditorUtility.SetDirty(thisTarget);
        }
    }
}