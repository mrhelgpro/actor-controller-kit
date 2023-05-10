using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace AssemblyActorCore
{
	/// <summary> 
	/// Check or creates: 
	/// <see cref="Camera"/>, 
	/// <see cref="CinemachineBrain"/>, 
	/// <see cref="ActorVirtualCamera"/>. 
	/// </summary>
	public class CameraBootstrap : BootstrapBehaviour
    {
        public override void UpdateBootstrap()
        {
			// Finds or creates a Camera
			Camera camera = FindAnyObjectByType<Camera>();

			if (camera == null)
			{
				GameObject instantiate = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
				instantiate.transform.position = new Vector3(0f, 0f, -10f);
				instantiate.transform.rotation = Quaternion.identity;
				camera = instantiate.GetComponent<Camera>();
			}

			camera.gameObject.tag = "MainCamera";
			camera.transform.parent = transform;

			// Finds or creates a Cinemachine Brain
			CinemachineBrain cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();

			if (cinemachineBrain == null)
			{
				cinemachineBrain = camera.gameObject.AddComponent<CinemachineBrain>();
				cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
				cinemachineBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;
			}

			// Finds or creates a Actor Virtual Camera
			ActorVirtualCamera actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();

			if (actorVirtualCamera == null)
			{
				ContextMenuExtention.CreatePrefab("Camera", "Actor Virtual Camera", notEditable: true);

				actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();
				actorVirtualCamera.gameObject.name = "Actor Virtual Camera";
			}

			actorVirtualCamera.transform.parent = transform;

			// Check Single Instance
			ActorExtantion.CheckSingleInstance<CameraBootstrap>();
			ActorExtantion.CheckSingleInstance<Camera>();
			ActorExtantion.CheckSingleInstance<CinemachineBrain>();
			ActorExtantion.CheckSingleInstance<ActorVirtualCamera>();
		}
    }

#if UNITY_EDITOR
	[ExecuteInEditMode]
	[CustomEditor(typeof(CameraBootstrap))]
	public class CameraBootstrapEditor : ModelEditor
	{
		public override void OnInspectorGUI()
		{
			bool error = false;

			if (ActorExtantion.CheckSingleInstance<CameraBootstrap>() == false)
			{
				DrawModelBox("<CameraBootstrap> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (ActorExtantion.CheckSingleInstance<Camera>() == false)
			{
				DrawModelBox("<Camera> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (ActorExtantion.CheckSingleInstance<CinemachineBrain>() == false)
			{
				DrawModelBox("<CinemachineBrain> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (ActorExtantion.CheckSingleInstance<ActorVirtualCamera>() == false)
			{
				DrawModelBox("<ActorVirtualCamera> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (error == false)
			{
				DrawHeader("CameraBootstrap");
			}
		}
	}
#endif
}