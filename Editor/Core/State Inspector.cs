using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(State))]
    public class StateInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            State thisTarget = (State)target;

            // Checking for a single instance in children and destroy duplicates
            if (CheckSingleInstanceOnObject<State>(thisTarget.gameObject) == false) return;

            bool error = false;

            // Check Actor
            Actor actorMachine = thisTarget.gameObject.GetComponentInParent<Actor>();
            if (actorMachine == null)
            {
                DrawModelBox("<Actor> - is not found", BoxStyle.Error);
                error = true;
            }

            // Check Activator
            Activator activator = thisTarget.gameObject.GetComponent<Activator>();
            if (activator == null)
            {
                DrawModelBox("<Activator> - is not found", BoxStyle.Error);
                error = true;
            }

            // Check Presenter
            Presenter presenter = thisTarget.gameObject.GetComponentInChildren<Presenter>();
            if (presenter == null)
            {
                DrawModelBox("<Presenter> - is not found", BoxStyle.Error);
                error = true;
            }

            if (error == false)
            {
                if (Application.isPlaying)
                {
                    DrawHeader(thisTarget.Name);
                    DrawHeader(thisTarget.Type.ToString(), 12);

                    if (thisTarget.IsCurrentState)
                    {
                        DrawModelBox("State active", BoxStyle.Active);
                    }
                    else
                    {
                        DrawModelBox("Waiting for state activation");
                    }
                }
                else
                {
                    thisTarget.Name = EditorGUILayout.TextField("Name", thisTarget.Name);
                    thisTarget.Type = (StateType)EditorGUILayout.EnumPopup("Type", thisTarget.Type);

                    DrawModelBox("Update the Presenter");
                }
            }

            EditorUtility.SetDirty(thisTarget);
        }
    }
}