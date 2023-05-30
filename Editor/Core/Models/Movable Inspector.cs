using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Movable))]
    [CanEditMultipleObjects]
    public class MovableInspector : ActormachineBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawSubtitle("CONTROLS SPEED");
        }
    }
}
