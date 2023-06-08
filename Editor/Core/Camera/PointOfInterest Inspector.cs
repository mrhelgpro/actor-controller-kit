using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace Actormachine.Editor
{
	[ExecuteInEditMode]
	[CustomEditor(typeof(PointOfInterest))]
	[CanEditMultipleObjects]
	public class PointOfInterestInspector : ActormachineComponentBaseInspector
	{
		public override void OnInspectorGUI()
		{
			PointOfInterest thisTarget = (PointOfInterest)target;

			bool error = false;

			// Check Cinemachin Brain
			CinemachineBrain cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();

			if (cinemachineBrain == null)
			{
				Inspector.DrawSubtitle("<Cinemachine Brain> - IS NOT FOUND", BoxStyle.Error);

				error = true;
			}

			// Check Actor Virtual Camera
			ActorVirtualCamera actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();

			if (actorVirtualCamera == null)
			{
				Inspector.DrawSubtitle("<Actor Virtual Camera> - IS NOT FOUND", BoxStyle.Error);

				error = true;
			}

			// Check Cinemachine Virtual Camera
			CinemachineVirtualCamera pointVirtualCamera = thisTarget.GetComponentInChildren<CinemachineVirtualCamera>();

			if (pointVirtualCamera == null)
			{
				Inspector.DrawSubtitle("<Cinemachine Virtual Camera> - IS NOT FOUND", BoxStyle.Error);

				error = true;
			}

			// Check Error
			if (error == true)
			{
				return;
			}

			// Draw a Inspector
			thisTarget.FieldOfView = EditorGUILayout.IntSlider("Field Of View", thisTarget.FieldOfView, 20, 80);
			thisTarget.Horizontal = EditorGUILayout.Slider("Horizontal", thisTarget.Horizontal, -180, 180);
			thisTarget.Vertical = EditorGUILayout.Slider("Vertical", thisTarget.Vertical, -90, 90);
			thisTarget.Distance = EditorGUILayout.Slider("Distance", thisTarget.Distance, 1, 20);
			thisTarget.EnterTime = EditorGUILayout.Slider("Enter Time", thisTarget.EnterTime, 0.1f, 5);
			thisTarget.ExitTime = EditorGUILayout.Slider("Exit Time", thisTarget.ExitTime, 0.1f, 2);
			thisTarget.ReturnToBack = EditorGUILayout.Toggle("Return To Back", thisTarget.ReturnToBack);

			if (GUI.changed)
			{
				Cinema.SwitchPriority(pointVirtualCamera);

				pointVirtualCamera.transform.rotation = Quaternion.Euler(thisTarget.Vertical, thisTarget.Horizontal, 0);
				pointVirtualCamera.transform.position = thisTarget.transform.rotation * Vector3.zero + pointVirtualCamera.Follow.transform.position;
				pointVirtualCamera.m_Lens.FieldOfView = thisTarget.FieldOfView;

				CinemachineFramingTransposer framingTransposer = pointVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
				framingTransposer.m_CameraDistance = thisTarget.Distance;
				pointVirtualCamera.UpdateCameraState(pointVirtualCamera.Follow.position, CinemachineCore.CurrentTime);
			}
		}
	}
}