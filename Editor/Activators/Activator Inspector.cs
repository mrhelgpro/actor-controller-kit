using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Activator))]
    public sealed class ActivatorInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Activator thisTarget = (Activator)target;

            // Checking for a single instance in children and destroy duplicates
            if (CheckSingleInstanceOnObject<Activator>(thisTarget.gameObject) == false) return;

            bool error = false;

            // Check Actor
            Actor actorMachine = thisTarget.gameObject.GetComponentInParent<Actor>();
            if (actorMachine == null)
            {
                DrawModelBox("<Actor> - is not found", BoxStyle.Error);
                error = true;
            }

            // Check State
            State state = thisTarget.gameObject.GetComponent<State>();
            if (state == null)
            {
                DrawModelBox("<State> - is not found", BoxStyle.Error);
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
                    DrawModelBox("Activated by free");
                }
            }

            EditorUtility.SetDirty(thisTarget);
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(ActivateByInput))]
    public class ActivateByInputInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            ActivateByInput thisTarget = (ActivateByInput)target;

            // Checking for a single instance in children and destroy duplicates
            if (CheckSingleInstanceOnObject<Activator>(thisTarget.gameObject) == false) return;

            DrawDefaultInspector();
        }
    }
}