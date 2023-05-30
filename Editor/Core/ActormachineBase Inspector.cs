using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorBehaviour))]
    [CanEditMultipleObjects]
    public class ActormachineBaseInspector : UnityEditor.Editor
    {
        private bool _foldoutInfo = false;

        public override void OnInspectorGUI()
        {
            DrawBaseInspector();
        }

        public void DrawBaseInspector()
        {
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }

        public void DrawInfoInspector()
        {
            GUILayout.BeginVertical();

            _foldoutInfo = EditorGUILayout.Foldout(_foldoutInfo, "Info");

            if (_foldoutInfo)
            {
                OnInfoInspector();
            }
            GUILayout.EndVertical();
        }

        public virtual void OnInfoInspector() { }
    }
}