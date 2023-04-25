using System;
using UnityEngine;
using UnityEditor;
          
namespace AssemblyActorCore
{
    public class Targetable : Model
    {
        private Target _target = null;

        public bool IsTarget => _target != null;
        public bool IsPosition => IsTarget && _target.IsPosition;
        public bool IsInteraction => IsTarget && _target.IsInteraction;
        public Vector3 GetPosition => IsTarget ? _target.GetPosition : Vector3.zero;
        public string GetTag => IsTarget ? _target.GetTag : "None";

        public void AddTarget(Vector3 position) => _target = new Target(position);
        public void AddTarget(Transform transform) => _target = new Target(transform);
        public void Clear() => _target = null;
    }

    [Serializable]
    public class Target
    {
        private Vector3 _position;
        private Transform _transform;

        public bool IsPosition => _transform == null;
        public bool IsInteraction => _transform != null;
        public Vector3 GetPosition => _transform == null ?_position : _transform.position;
        public string GetTag => _transform == null ? "Position" : _transform.tag;

        public Target(Vector3 position)
        {
            _position = position;
        }

        public Target(Transform transform)
        {
            _position = transform.position;
            _transform = transform;
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Targetable))]
    public class TargetableCameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Targetable myTarget = (Targetable)target;

            if (Application.isPlaying)
            {
                if (myTarget.IsTarget)
                {
                    EditorGUILayout.LabelField("Target Tag: " + myTarget.GetTag);
                    EditorGUILayout.Vector3Field("", myTarget.GetPosition);
                }
                else
                {
                    EditorGUILayout.LabelField("Target - Null");
                }

                EditorUtility.SetDirty(target);
            }
            else
            {
                myTarget.Clear();
            }
        }
    }
#endif
}