using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Positionable))]
    [CanEditMultipleObjects]
    public class PositionableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Positionable thisTarget = (Positionable)target;

            if (Application.isPlaying == false)
            {
                Inspector.DrawInfoBox("CHECK POSITION DATA");
            }
            else
            {
                Inspector.DrawInfoBox("GROUND: " + thisTarget.SurfaceType, thisTarget.IsGrounded ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("OBSTACLE: " + (thisTarget.ObstacleTransform == null ? "None" : thisTarget.ObstacleTransform.name), thisTarget.IsObstacle ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("EDGE", thisTarget.IsEdge ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("ABYSS", thisTarget.IsAbyss ? BoxStyle.Active : BoxStyle.Default);
            }
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(Positionable2D))]
    [CanEditMultipleObjects]
    public class Positionable2DInspector : PositionableInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
