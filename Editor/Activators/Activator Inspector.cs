using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Activator))]
    public class ActivatorInspector : ActorBehaviourInspector
    {
        public void DrawActivator()
        {
            Activator thisTarget = (Activator)target;

            // Checking for a single instance in children and destroy duplicates
            if (CheckSingleInstanceOnObject<Activator>(thisTarget.gameObject) == false) return;

            // Check Presenter
            Presenter presenter = thisTarget.gameObject.GetComponent<Presenter>();
            if (presenter == null)
            {
                DrawModelBox("<Presenter> - is not found", BoxStyle.Error);
                return;
            }
        }

        public override void OnInspectorGUI()
        {
            DrawActivator();

            DrawModelBox("Activated by free");
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(ActivateByInput))]
    public class ActivateByInputInspector : ActivatorInspector
    {
        public override void OnInspectorGUI()
        {
            DrawActivator();

            DrawDefaultInspector();
        }
    }
}