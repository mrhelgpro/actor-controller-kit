using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(State))]
    public sealed class StateInspector : ActorBehaviourInspector
    {
        public void OnEnable()
        {
            //StateBehaviour thisTarget = (StateBehaviour)target;
            //thisTarget.UpdateInspector();
        }

        public override void OnInspectorGUI()
        {
            State thisTarget = (State)target;

            // Checking for a single instance in children and destroy duplicates
            if (CheckSingleInstanceOnObject<State>(thisTarget.gameObject) == false) return;

            // Check Presenter
            Presenter presenter = thisTarget.gameObject.GetComponent<Presenter>();
            if (presenter == null)
            {
                DrawModelBox("<Presenter> - is not found", BoxStyle.Error);
                return;
            }

            if (Application.isPlaying)
            {
                DrawHeader(thisTarget.Name);
                DrawHeader(thisTarget.Type.ToString(), 12);

                Actor actor = thisTarget.gameObject.GetComponentInParent<Actor>();

                if (actor.IsCurrentState(thisTarget))
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

            EditorUtility.SetDirty(thisTarget);
        }
    }
}