using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    /// <summary> 
    /// Model for interfacing with the Actor Virtual Camera. 
    /// There should be only one on the scene.
    /// </summary>
    public class Followable : ActorComponent
    {
        public ActorVirtualCamera ActorVirtualCamera;

        // Check Single Instance <Followable>
        private void OnValidate() => ActorExtantion.CheckSingleInstance<Followable>();

        private new void Awake()
        {
            base.Awake();

            FindActorVirtualCamera();
        }

        /// <summary> Finds or creates a Actor Virtual Camera. </summary>
        public void FindActorVirtualCamera()
        {
            ActorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();

            if (ActorVirtualCamera == null)
            {
                ContextMenuExtention.CreatePrefab("Camera", "Actor Virtual Camera (Null)", notEditable: true);

                ActorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();
                ActorVirtualCamera.gameObject.name = "Actor Virtual Camera(" + FindRootTransform.name + ")";
            }
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
            Followable thisTarget = (Followable)target;

            if (thisTarget.ActorVirtualCamera == null)
            {
                thisTarget.FindActorVirtualCamera();
            }

            if (ActorExtantion.CheckSingleInstance<Followable>() == true)
            {
                if (ActorExtantion.CheckSingleInstance<ActorVirtualCamera>() == true)
                {
                    DrawModelBox("Edited in the Presenter");

                    if (Application.isPlaying == false)
                    {
                        // Update Virtual Camera Parameters
                        thisTarget.ActorVirtualCamera.gameObject.name = "Actor Virtual Camera (" + thisTarget.FindRootTransform.name + ")";
                        thisTarget.ActorVirtualCamera.Enter(thisTarget.transform, thisTarget.ActorVirtualCamera.Parameters);
                    }
                }
                else
                {
                    DrawModelBox("Should be a single instance of <ActorVirtualCamera>", BoxStyle.Error);
                }
            }
            else
            {
                DrawModelBox("Should be a single instance of <Followable>", BoxStyle.Error);
            }
        }
    }
#endif
}