using System;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    [ExecuteInEditMode]
    public class TargetForCamera : MonoBehaviour
    {
        public ActorCameraSettings Settings = new ActorCameraSettings();
        [HideInInspector] public Transform Transform;

        protected void Awake() => Transform = transform;
    }

    [Serializable]
    public class ActorCameraSettings
    {
        [Range(-30, 90)] public float Vertical = 0;
        [Range(-180, 180)] public float Horizontal = 0;
        [Range(0, 5)] public float Height = 1;
        [Range(-1, 1)] public float Shoulder = 0;
        [Range(1, 15)] public int Distance = 5;
        [Range(0.1f, 1.0f)] public float MoveTime = 0.5f;
        [Range(0.1f, 1.0f)] public float RotationTime = 0.5f;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(TargetForCamera))]
    public class TargetForCameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Attach this object to the <ActorCamera>");
            EditorGUILayout.LabelField("And edit the camera position");
        }
    }
#endif
}