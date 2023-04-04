using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class TargetForLook : TargetForCamera
    {
        protected Input input => _inputable.Input;
        private Inputable _inputable;    

        private new void Awake()
        {
            base.Awake();

            _inputable = GetComponentInParent<Inputable>();

            Settings.MoveTime = 0.01f;
            Settings.RotationTime = 0.05f;
        }

        private void Update()
        {
            Settings.Horizontal = input.Look.x;
            Settings.Vertical = input.Look.y;
            Settings.Vertical = Mathf.Clamp(Settings.Vertical, -30, 85);
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(TargetForLook))]
    public class TargetForLookEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Attach this object to the <ActorCamera>");
            EditorGUILayout.LabelField("And edit the camera position");
        }
    }
#endif
}