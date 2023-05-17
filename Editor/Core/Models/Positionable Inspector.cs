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
                Inspector.DrawModelBox("Ñhecks position data");
            }
            else
            {
                Inspector.DrawModelBox("Ground: " + thisTarget.SurfaceType, thisTarget.IsGrounded ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawModelBox("Obstacle: " + (thisTarget.ObstacleTransform == null ? "None" : thisTarget.ObstacleTransform.name), thisTarget.IsObstacle ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawModelBox("Edge", thisTarget.IsEdge ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawModelBox("Abyss", thisTarget.IsAbyss ? BoxStyle.Active : BoxStyle.Default);
            }
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(Positionable2D))]
    public class Positionable2DInspector : PositionableInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
