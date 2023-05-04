using System;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    /// <summary> Model - to control the Camera Parametres. </summary>
    public class Followable : MonoBehaviour
    {
        public CameraParametres Parametres;

        public Transform ThisTransform { get; private set; }

        private void Awake() => ThisTransform = transform;

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
                Debug.LogWarning(gameObject.name + " - Followable: is not found <ActorCamera>");
            }
        }
    }

    public enum InputOrbitMode { Free, LeftHold, MiddleHold, RightHold, Freez }
    public enum InputLookMode { Free, LeftHold, MiddleHold, RightHold, Freez }
    public enum InputOffsetMode { Free, LeftHold, MiddleHold, RightHold, Freez }

    /// <summary> Camera position and settings. </summary>
    [Serializable]
    public class CameraParametres
    {
        public InputOrbitMode InputOrbitMode;
        //public InputLookMode InputLookMode;
        //public InputOffsetMode InputOffsetMode;

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
    public class FollowableEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {   
            Followable thisTarget = (Followable)target;

            // Parametres Structure
            if (thisTarget.GetComponentInParent<ActorComponent>() == null)
            {
                if (Application.isPlaying == false)
                {
                    SerializedProperty parametresProperty = serializedObject.FindProperty("Parametres");
                    EditorGUILayout.PropertyField(parametresProperty, true);
                    serializedObject.ApplyModifiedProperties();

                    if (GUI.changed)
                    {
                        thisTarget.SetPreview(thisTarget.Parametres);
                        EditorUtility.SetDirty(thisTarget);
                    }
                }
            }
            else
            {
                DrawModelBox("Edited in the Presenter");
            }
        }
    }
#endif
}