using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Activator))]
    [CanEditMultipleObjects]
    public class ActivatorInspector : ActorBehaviourInspector
    {
        public void DrawActivator()
        {
            Activator thisTarget = (Activator)target;

            // Check Presenter
            Presenter presenter = thisTarget.gameObject.GetComponent<Presenter>();
            if (presenter == null)
            {
                Inspector.DrawModelBox("<Presenter> - is not found", BoxStyle.Error);
                return;
            }
        }

        public override void OnInspectorGUI()
        {
            DrawActivator();

            Inspector.DrawModelBox("Activated by free");
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(ActivatorByInput))]
    public class ActivatorByInputInspector : ActivatorInspector
    {
        public override void OnInspectorGUI()
        { 
            DrawActivator();

            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }
    }
}