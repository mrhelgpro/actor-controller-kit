using UnityEngine;

namespace AssemblyActorCore
{
    /// <summary> Model - for speed control. </summary>
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
        public override void OnInspectorGUI()
        {
            DrawModelBox("Controls speed");
        }
    }
#endif
}