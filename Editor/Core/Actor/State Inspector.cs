using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(State))]
    [CanEditMultipleObjects]
    public sealed class StateInspector : ActorBehaviourInspector
    {
        public void OnEnable()
        {
            StateBehaviour thisTarget = (StateBehaviour)target;
            thisTarget.InitiationState();
        }

        public override void OnInspectorGUI()
        {
            State thisTarget = (State)target;

            // Checking for a single instance in children and destroy duplicates
            if (thisTarget.gameObject.CheckSingleInstanceOnObject<State>() == false) return;

            // Check Presenter
            Presenter presenter = thisTarget.gameObject.GetComponent<Presenter>();
            if (presenter == null)
            {
                Inspector.DrawModelBox("<Presenter> - is not found", BoxStyle.Error);
                return;
            }

            if (Application.isPlaying)
            {
                Inspector.DrawHeader(thisTarget.Name);
                Inspector.DrawHeader(thisTarget.Type.ToString(), 12);

                Actor actor = thisTarget.gameObject.GetComponentInParent<Actor>();

                if (actor.IsCurrentState(thisTarget))
                {
                    Inspector.DrawModelBox("State active", BoxStyle.Active);
                }
                else
                {
                    Inspector.DrawModelBox("Waiting for state activation");
                }
            }
            else
            {
                thisTarget.Name = EditorGUILayout.TextField("Name", thisTarget.Name);
                thisTarget.Type = (StateType)EditorGUILayout.EnumPopup("Type", thisTarget.Type);

                Inspector.DrawModelBox("Update the Presenter");
            }

            EditorUtility.SetDirty(thisTarget);
        }
    }
}