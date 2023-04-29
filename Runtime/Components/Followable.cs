using System;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class Followable : ModelComponent
    {
        public CameraSettings Settings;

        public void SetPreview(CameraSettings settings)
        {
            ActorCamera actorCamera = FindObjectOfType<ActorCamera>();

            if (actorCamera)
            {
                Settings = settings;
                actorCamera.PreviewTheTarget(this);
            }
            else
            {
                Debug.LogWarning(gameObject.name + " - Followable: <ActorCamera> - is not found");
            }
        }

        public void SetParametres(CameraSettings settings, Vector2 look, bool isRotable)
        {
            Settings.Offset = settings.Offset;
            Settings.DampTime = settings.DampTime;
            Settings.FieldOfView = settings.FieldOfView;

            if (isRotable)
            {
                Settings.Orbit.Horizontal += look.x * settings.Sensitivity.Horizontal;
                Settings.Orbit.Vertical += look.y * settings.Sensitivity.Vertical;
                Settings.Orbit.Vertical = Mathf.Clamp(Settings.Orbit.Vertical, -30, 80);

                return;
            }

            Settings.Orbit = settings.Orbit;
        }
    }

    public enum InputCameraMode { Free, LeftHold, MiddleHold, RightHold, Freez }

    [Serializable]
    public struct CameraSettings
    {
        public InputCameraMode InputCameraMode;
        public Vector2Orbit Orbit;
        public Vector2Sensitivity Sensitivity;
        public Vector3Offset Offset;
        public Vector2DampTime DampTime;

        [Range(10, 90)] public int FieldOfView;
    }

    [Serializable]
    public struct Vector2Orbit
    {
        [Range(-180, 180)] public float Horizontal;
        [Range(-90, 90)] public float Vertical;
    }

    [Serializable]
    public struct Vector2Sensitivity
    {
        [Range(0, 5)] public float Horizontal;
        [Range(0, 5)] public float Vertical;
    }

    [Serializable]
    public struct Vector3Offset
    {
        [Range(-5, 5)] public float Horizontal;
        [Range(-5, 5)] public float Vertical;
        [Range(0, 20)] public float Distance;
    }

    [Serializable]
    public struct Vector2DampTime
    {
        [Range(0, 1)] public float Move;
        [Range(0, 1)] public float Rotation;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Followable))]
    public class FollowableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Followable myTarget = (Followable)target;

            // Script Link
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Model", MonoScript.FromMonoBehaviour(myTarget), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            Rect scriptRect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(scriptRect, MouseCursor.Arrow);

            if (GUI.Button(scriptRect, "", GUIStyle.none))
            {
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(myTarget)), 0);
            }

            // Settings Structure
            if (myTarget.GetComponent<Actor>() == null)
            {
                if (Application.isPlaying == false)
                {
                    SerializedProperty settingsProperty = serializedObject.FindProperty("Settings");
                    EditorGUILayout.PropertyField(settingsProperty, true);
                    serializedObject.ApplyModifiedProperties();

                    if (GUI.changed)
                    {
                        myTarget.SetPreview(myTarget.Settings);
                    }
                }
            }
        }
    }
#endif
}