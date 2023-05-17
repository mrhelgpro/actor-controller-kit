using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(MovementPhysicPresenter))]
    [CanEditMultipleObjects]
    public class MovementPhysicPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(MovementNavigationPresenter))]
    public class MovementNavigationPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(Movement2DPresenter))]
    public class Movement2DPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }
    }
}