using System;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class Followable : ModelComponent
    {
        public ActorCameraSettings Settings;

        public void SetPreview(ActorCameraSettings settings)
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

        public void SetParametres(ActorCameraSettings settings, Vector2 look)
        {
            if (settings.InputCameraMode == InputCameraMode.Free)
            { 
                // Do somethings
            }

            Settings.HorizontalDirection += look.x * settings.HorizontalSensitivity;
            Settings.VerticalDirection += look.y * settings.VerticalSensitivity;
            Settings.VerticalDirection = Mathf.Clamp(Settings.VerticalDirection, -25, 80);

            Settings.Height = settings.Height;
            Settings.Shoulder = settings.Shoulder;
            Settings.Distance = settings.Distance;
            Settings.FieldOfView = settings.FieldOfView;
            Settings.MoveTime = settings.MoveTime;
            Settings.RotationTime = settings.RotationTime;
        }
    }

    public enum InputCameraMode { Free, LeftHold, RightHold, MiddleHold}

    [Serializable]
    public struct ActorCameraSettings
    {
        public InputCameraMode InputCameraMode;
        [Range(-180, 180)] public float HorizontalDirection;
        [Range(-90, 90)] public float VerticalDirection;
        [Range(0, 5)] public float HorizontalSensitivity;
        [Range(0, 5)] public float VerticalSensitivity;
        [Range(-5, 5)] public float Height;
        [Range(-1, 1)] public float Shoulder;
        [Range(0, 15)] public float Distance;
        [Range(10, 90)] public int FieldOfView;
        [Range(0.01f, 1.0f)] public float MoveTime;
        [Range(0.05f, 1.0f)] public float RotationTime;
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