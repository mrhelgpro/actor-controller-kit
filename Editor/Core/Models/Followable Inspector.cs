using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Followable))]
    public class FollowableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawModelBox("Mark for Camera");
        }
    }
}