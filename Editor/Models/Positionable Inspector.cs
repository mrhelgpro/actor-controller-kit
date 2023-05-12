using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Positionable))]
    public class PositionableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            DrawModelBox("�hecks position data");
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(Positionable2D))]
    public class Positionable2DInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            DrawModelBox("�hecks 2D position data");
        }
    }
}
