using UnityEngine;
using Cinemachine;

namespace Actormachine
{
	/// <summary> 
	/// Check or creates: 
	/// <see cref="Camera"/>, 
	/// <see cref="CinemachineBrain"/>, 
	/// <see cref="ActorVirtualCamera"/>. 
	/// </summary>
	public class CameraBootstrap : Bootstrap
    {
        public override void Initiation()
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

			camera.gameObject.name = "Main Camera (Cinemachine Brain)";
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
				GameObject instantiate = new GameObject("Cinemachine Virtual Camera", typeof(CinemachineVirtualCamera), typeof(ActorVirtualCamera));
				instantiate.transform.position = new Vector3(0f, 0f, -10f);
				instantiate.transform.rotation = Quaternion.identity;
				instantiate.hideFlags = HideFlags.NotEditable;
				actorVirtualCamera = instantiate.GetComponent<ActorVirtualCamera>();
			}

			actorVirtualCamera.gameObject.name = "Cinemachine Virtual Camera (Actor)";
			actorVirtualCamera.transform.parent = transform;

			// Check Single Instance
			ActorExtantion.IsSingleInstanceOnScene<CameraBootstrap>();
			ActorExtantion.IsSingleInstanceOnScene<Camera>();
			ActorExtantion.IsSingleInstanceOnScene<CinemachineBrain>();
			ActorExtantion.IsSingleInstanceOnScene<ActorVirtualCamera>();
		}
    }
}