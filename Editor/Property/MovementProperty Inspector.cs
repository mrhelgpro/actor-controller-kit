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
            base.OnInspectorGUI();
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