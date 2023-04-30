using System;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class Followable : ModelComponent
    {
        public CameraParametres Parametres;

        public void SetPreview(CameraParametres parametres)
        {
            ActorCamera actorCamera = FindObjectOfType<ActorCamera>();

            if (actorCamera)
            {
                Parametres = parametres;
                actorCamera.PreviewTheTarget(this);
            }
            else
            {
                Debug.LogWarning(gameObject.name + " - Followable: <ActorCamera> - is not found");
            }
        }
    }

    public enum InputCameraMode { Free, LeftHold, MiddleHold, RightHold, Freez }

    [Serializable]
    public class CameraParametres
    {
        public InputCameraMode InputCameraMode;
        public Vector2Orbit Orbit = new Vector2Orbit(1.0f, 0.5f);
        public Vector3Offset Offset = new Vector3Offset(0.0f, 0.0f, 5.0f);
        public Vector2DampTime DampTime;

        [Range(10, 90)] public int FieldOfView = 60;
    }

    [Serializable]
    public struct Vector2Orbit
    {
        [Range(-180, 180)] public float Horizontal;
        [Range(-90, 90)] public float Vertical;
        [Range(0, 5)] public float SensitivityX;
        [Range(0, 5)] public float SensitivityY;

        public Vector2Orbit(float x, float y)
        {
            Horizontal = 0;
            Vertical = 0;
            SensitivityX = x;
            SensitivityY = y;
        }
    }

    [Serializable]
    public struct Vector3Offset
    {
        [Range(-5, 5)] public float Horizontal;
        [Range(-5, 5)] public float Vertical;
        [Range(0, 20)] public float Distance;

        public Vector3Offset(float horizontal, float vertical, float distance)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            Distance = distance;
        }
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

            // Parametres Structure
            if (myTarget.GetComponent<Actor>() == null)
            {
                if (Application.isPlaying == false)
                {
                    SerializedProperty parametresProperty = serializedObject.FindProperty("Parametres");
                    EditorGUILayout.PropertyField(parametresProperty, true);
                    serializedObject.ApplyModifiedProperties();

                    if (GUI.changed)
                    {
                        myTarget.SetPreview(myTarget.Parametres);
                    }
                }
            }
        }
    }
#endif
}