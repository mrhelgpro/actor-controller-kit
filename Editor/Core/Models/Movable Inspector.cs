using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Movable))]
    [CanEditMultipleObjects]
    public class MovableInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawSubtitle("CONTROLS SPEED");

            DrawBaseInspector();
        }
    }
}
