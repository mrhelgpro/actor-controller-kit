using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Followable))]
    [CanEditMultipleObjects]
    public class FollowableInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawSubtitle("MARK FOR CAMERA");
        }
    }
}