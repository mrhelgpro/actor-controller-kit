using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace Actormachine.Editor
{
	[ExecuteInEditMode]
	[CustomEditor(typeof(BootstrapCamera))]
	public class CameraBootstrapInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			// Draw a Warning	
			bool error = false;

			if (BootstrapExtantion.IsSingleInstanceOnScene<BootstrapCamera>() == false)
			{
				Inspector.DrawModelBox("<CameraBootstrap> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (BootstrapExtantion.IsSingleInstanceOnScene<Camera>() == false)
			{
				Inspector.DrawModelBox("<Camera> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (BootstrapExtantion.IsSingleInstanceOnScene<CinemachineBrain>() == false)
			{
				Inspector.DrawModelBox("<CinemachineBrain> should be a single", BoxStyle.Warning);

				error = true;
			}

			if (BootstrapExtantion.IsSingleInstanceOnScene<ActorVirtualCamera>() == false)
			{
				Inspector.DrawModelBox("<ActorVirtualCamera> should be a single", BoxStyle.Warning);

				error = true;
			}

			// Draw a Inspector
			if (error == false)
			{
				Inspector.DrawHeader("CameraBootstrap");

				// Show Main Camera
				CinemachineBrain mainCamera = FindAnyObjectByType<CinemachineBrain>();
				CinemachineVirtualCameraBase liveCamera = mainCamera.ActiveVirtualCamera as CinemachineVirtualCameraBase;
				Inspector.DrawLinkButton("Main Camera", mainCamera.gameObject, ButtonStyle.Main);

				// Show Actor Virtual Camera
				ActorVirtualCamera actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();
				CinemachineVirtualCameraBase actorCinemachineVirtualCamera = actorVirtualCamera.GetComponent<CinemachineVirtualCameraBase>();
				ButtonStyle actorStyle = liveCamera == actorCinemachineVirtualCamera ? ButtonStyle.Active : ButtonStyle.Default;
				Inspector.DrawLinkButton("Actor Virtual Camera", actorVirtualCamera.gameObject, actorStyle);

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
									Inspector.DrawLinkButton(cinemachineVirtualCamera.transform.parent.gameObject.name, cinemachineVirtualCamera.transform.parent.gameObject, buttonStyle);
								}
							}
							else
							{
								Inspector.DrawLinkButton(cinemachineVirtualCamera.gameObject.name, cinemachineVirtualCamera.gameObject, buttonStyle);
							}
						}
					}
				}

				EditorUtility.SetDirty(target);
			}
		}
	}
}