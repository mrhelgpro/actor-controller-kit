using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(DirectionProperty))]
    [CanEditMultipleObjects]
    public class DirectionPropertyInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
