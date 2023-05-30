using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(MovementPhysicProperty))]
    [CanEditMultipleObjects]
    public class MovementPhysicPropertyInspector : ActormachineBaseInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(MovementNavigationProperty))]
    [CanEditMultipleObjects]
    public class MovementNavigationPropertyInspector : ActormachineBaseInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(Movement2DProperty))]
    [CanEditMultipleObjects]
    public class Movement2DPropertyInspector : ActormachineBaseInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}