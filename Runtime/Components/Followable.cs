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

        public void SetParametres(ActorCameraSettings settings, ref InputVector look)
        {
            Settings = settings;

            if (look.Sensitivity > 0)
            {
                Settings.Horizontal = look.Value.x;
                look.Value.y = Mathf.Clamp(look.Value.y, -30, 85);
                Settings.Vertical = look.Value.y;
            }
        }
    }

    [Serializable]
    public struct ActorCameraSettings
    {
        [Range(-30, 90)] public float Vertical;
        [Range(-180, 180)] public float Horizontal;
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