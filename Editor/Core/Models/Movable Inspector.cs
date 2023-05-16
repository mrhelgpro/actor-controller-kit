using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Movable))]
    public class MovableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawModelBox("Controls speed");
        }
    }
}
