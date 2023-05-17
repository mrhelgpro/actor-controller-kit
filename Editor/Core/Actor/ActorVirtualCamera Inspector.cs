using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorVirtualCamera))]
    [CanEditMultipleObjects]
    public class ActorVirtualCameraInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            ActorVirtualCamera thisTarget = (ActorVirtualCamera)target;

            thisTarget.gameObject.hideFlags = HideFlags.NotEditable;

            // Draw a Warning
            if (Inspector.CheckSingleInstanceOnScene<BootstrapCamera>()) return;

            // Draw a Inspector

            if (thisTarget.IsLock == true)
            {
                Inspector.DrawModelBox("Locked", BoxStyle.Warning);
            }
            else
            {
                Inspector.DrawModelBox("Edited in the Presenter");
            }
        }
    }
}