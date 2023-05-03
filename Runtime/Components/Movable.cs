using UnityEngine;

namespace AssemblyActorCore
{
    public class Movable : ActorComponent
    {
        // Movable Fields
        private float _speedScale = 1;
        private float _gravityScale = 1;

        // Return Value
        public float GetSpeed(float value) => (_speedScale < 0 ? 0 : _speedScale) * value;
        public float GetGravity(float value) => (_gravityScale < 0 ? 0 : _gravityScale) * value;

        // Change Value
        public void ChangeSpeed(float value) => _speedScale += value;
        public void ChangeGravity(float value) => _gravityScale += value;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [UnityEditor.CustomEditor(typeof(Movable))]
    public class MovableEditor : ModelEditor
    {
        protected override void OnHeaderGUI()
        {
            GUILayout.Label("My Custom Header");
            UnityEditor.EditorGUILayout.LabelField("Header!");
        }

        public override void OnInspectorGUI()
        {
            DefaultModelStyle("Movable - for speed control");
        }
    }
#endif
}