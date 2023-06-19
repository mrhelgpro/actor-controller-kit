using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Positionable))]
    [CanEditMultipleObjects]
    public class PositionableInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawSubtitle("CHECK POSITION DATA");
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(Positionable2D))]
    [CanEditMultipleObjects]
    public class Positionable2DInspector : PositionableInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(PositionableNavigation))]
    [CanEditMultipleObjects]
    public class PositionableNavigationInspector : PositionableInspector
    {
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}
