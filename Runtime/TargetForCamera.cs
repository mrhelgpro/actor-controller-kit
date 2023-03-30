using System;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    [ExecuteInEditMode]
    public class TargetForCamera : MonoBehaviour
    {
        public ActorCameraSettings Settings;
        public Transform Transform;

        private void Awake() => Transform = transform;
    }

    [Serializable]
    public class ActorCameraSettings
    {
        [Range(0, 90)] public int Angle = 0;
        [Range(0, 5)] public float Height = 1;
        [Range(1, 15)] public int Distance = 5;
        [Range(0.1f, 1)] public float MoveTime = 0.5f;
        [Range(0.1f, 1)] public float RotationTime = 0.5f;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(TargetForCamera))]
    public class TargetForCameraaEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Attach this object to the <ActorCamera>");
            EditorGUILayout.LabelField("And edit the camera position");
        }
    }
#endif
}