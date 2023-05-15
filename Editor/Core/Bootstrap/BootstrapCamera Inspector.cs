using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace Actormachine.Editor
{
	[ExecuteInEditMode]
	[CustomEditor(typeof(BootstrapCamera))]
	public class CameraBootstrapInspector : ActorBehaviourInspector
	{
		public override void OnInspectorGUI()
		{
			// Draw a Warning	
			bool error = false;

			if (BootstrapExtantion.IsSingleInstanceOnScene<BootstrapCamera>() == false)
			{
				DrawModelBox("<CameraBootstrap> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (BootstrapExtantion.IsSingleInstanceOnScene<Camera>() == false)
			{
				DrawModelBox("<Camera> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (BootstrapExtantion.IsSingleInstanceOnScene<CinemachineBrain>() == false)
			{
				DrawModelBox("<CinemachineBrain> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (BootstrapExtantion.IsSingleInstanceOnScene<ActorVirtualCamera>() == false)
			{
				DrawModelBox("<ActorVirtualCamera> should be a single", BoxStyle.Warning);

				error = true;
			}

			// Draw a Inspector
			if (error == false)
			{
				DrawHeader("CameraBootstrap");

				// Show Main Camera
				CinemachineBrain mainCamera = FindAnyObjectByType<CinemachineBrain>();
				CinemachineVirtualCameraBase liveCamera = mainCamera.ActiveVirtualCamera as CinemachineVirtualCameraBase;
				DrawLinkButton("Main Camera", mainCamera.gameObject, ButtonStyle.Main);

				// Show Actor Virtual Camera
				ActorVirtualCamera actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();
				CinemachineVirtualCameraBase actorCinemachineVirtualCamera = actorVirtualCamera.GetComponent<CinemachineVirtualCameraBase>();
				ButtonStyle actorStyle = liveCamera == actorCinemachineVirtualCamera ? ButtonStyle.Active : ButtonStyle.Default;
				DrawLinkButton("Actor Virtual Camera", actorVirtualCamera.gameObject, actorStyle);

				// Show Other Virtual Cameras
				CinemachineVirtualCameraBase[] cinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCameraBase>();

				if (cinemachineVirtualCameras.Length > 0)
				{
					foreach (CinemachineVirtualCameraBase cinemachineVirtualCamera in cinemachineVirtualCameras)
					{
						ButtonStyle buttonStyle = liveCamera == cinemachineVirtualCamera ? ButtonStyle.Active : ButtonStyle.Default;

						if (cinemachineVirtualCamera.GetComponent<ActorVirtualCamera>() == null)
						{
							if (cinemachineVirtualCamera.gameObject.hideFlags == (HideFlags.HideInHierarchy | HideFlags.HideInInspector))
							{
								if (cinemachineVirtualCamera.transform.parent != null)
								{
									DrawLinkButton(cinemachineVirtualCamera.transform.parent.gameObject.name, cinemachineVirtualCamera.transform.parent.gameObject, buttonStyle);
								}
							}
							else
							{
								DrawLinkButton(cinemachineVirtualCamera.gameObject.name, cinemachineVirtualCamera.gameObject, buttonStyle);
							}
						}
					}
				}

				EditorUtility.SetDirty(target);
			}
		}

		private bool isLiveCamera(CinemachineVirtualCameraBase camera)
		{
			CinemachineBrain cinemachineBrain = FindObjectOfType<CinemachineBrain>();
			if (cinemachineBrain != null)
			{
				CinemachineVirtualCameraBase liveCamera = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCameraBase;
				return (liveCamera == camera);
			}
			return false;
		}
	}
}