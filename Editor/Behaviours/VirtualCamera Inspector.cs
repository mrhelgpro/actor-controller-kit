using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorVirtualCamera))]
    public class VirtualCameraInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            ActorVirtualCamera thisTarget = (ActorVirtualCamera)target;

            thisTarget.gameObject.hideFlags = HideFlags.NotEditable;

            // Draw a Warning
            if (CheckSingleInstanceOnScene<CameraBootstrap>()) return;

            // Draw a Inspector

            if (thisTarget.IsLock == true)
            {
                DrawModelBox("Locked", BoxStyle.Warning);
            }
            else
            {
                DrawModelBox("Edited in the Presenter");
            }
        }
    }
}