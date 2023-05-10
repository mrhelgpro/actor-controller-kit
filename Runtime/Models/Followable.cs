using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace AssemblyActorCore
{
    /// <summary> 
    /// Model for interfacing with the Actor Virtual Camera. 
    /// There should be only one on the scene.
    /// </summary>
    public class Followable : ActorBehaviour
    {
        public ActorVirtualCamera ActorVirtualCamera;

        private new void Awake()
        {
            base.Awake();

            FindActorVirtualCamera();
        }

        /// <summary> Finds or creates a Actor Virtual Camera. </summary>
        public void FindActorVirtualCamera()
        {
            ActorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();
        }

        /// <summary> Entering with setting Camera Parameters. It is not recommended to call in Update.</summary>
        public void Enter(CameraParameters enterParameters, bool isPreview = false) => ActorVirtualCamera?.Enter(transform, enterParameters, isPreview);
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Followable))]
    public class FollowableEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            if (CheckBootstrap<CameraBootstrap>()) return;

            DrawModelBox("Edited in the Presenter");
        }
    }
#endif
}