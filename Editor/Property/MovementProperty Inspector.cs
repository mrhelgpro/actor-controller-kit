using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(MovementPhysicProperty))]
    [CanEditMultipleObjects]
    public class MovementPhysicPropertyInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            // Draw a Inspector
            MovementPhysicProperty thisTarget = (MovementPhysicProperty)target;

            Movable movable = thisTarget.GetComponentInParent<Movable>();

            string walkValue = movable == null ? "" : " (" + movable.WalkSpeed * thisTarget.WalkScale + ")";
            string runValue = movable == null ? "" : " (" + movable.RunSpeed * thisTarget.RunScale + ")";
            string jumpValue = movable == null ? "" : " (" + movable.JumpHeight * thisTarget.JumpScale + ")";

            thisTarget.WalkScale = EditorGUILayout.Slider("Walk Scale" + walkValue, thisTarget.WalkScale, 0, 1);
            thisTarget.RunScale = EditorGUILayout.Slider("Run Scale" + runValue, thisTarget.RunScale, 0, 1);
            thisTarget.JumpScale = EditorGUILayout.Slider("Jump Scale" + jumpValue, thisTarget.JumpScale, 0, 1);
            thisTarget.Rate = EditorGUILayout.IntSlider("Rate", thisTarget.Rate, 0, 10);

            thisTarget.WalkScale = Mathf.Round(thisTarget.WalkScale * 100f) / 100f;
            thisTarget.RunScale = Mathf.Round(thisTarget.RunScale * 100f) / 100f;
            thisTarget.JumpScale = Mathf.Round(thisTarget.JumpScale * 10f) / 10f;
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(MovementNavigationProperty))]
    [CanEditMultipleObjects]
    public class MovementNavigationPropertyInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(Movement2DProperty))]
    [CanEditMultipleObjects]
    public class Movement2DPropertyInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}