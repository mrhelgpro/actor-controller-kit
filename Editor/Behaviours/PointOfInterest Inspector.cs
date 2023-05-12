using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace Actormachine.Editor
{
	[ExecuteInEditMode]
	[CustomEditor(typeof(PointOfInterest))]
	public class PointOfInterestInspector : ActorBehaviourInspector
	{
		public override void OnInspectorGUI()
		{
			// Draw a Warning
			if (CheckSingleInstanceOnScene<CameraBootstrap>()) return;

			// Draw a Inspector
			PointOfInterest thisTarget = (PointOfInterest)target;

			CinemachineVirtualCamera pointVirtualCamera = thisTarget.GetComponentInChildren<CinemachineVirtualCamera>();

			thisTarget.FieldOfView = EditorGUILayout.IntSlider("Field Of View", thisTarget.FieldOfView, 20, 80);
			thisTarget.Horizontal = EditorGUILayout.Slider("Horizontal", thisTarget.Horizontal, -180, 180);
			thisTarget.Vertical = EditorGUILayout.Slider("Vertical", thisTarget.Vertical, -90, 90);
			thisTarget.Distance = EditorGUILayout.Slider("Distance", thisTarget.Distance, 1, 20);
			thisTarget.EnterTime = EditorGUILayout.Slider("Enter Time", thisTarget.EnterTime, 0.1f, 5);
			thisTarget.ExitTime = EditorGUILayout.Slider("Exit Time", thisTarget.ExitTime, 0.1f, 2);
			thisTarget.ReturnToBack = EditorGUILayout.Toggle("Return To Back", thisTarget.ReturnToBack);

			if (GUI.changed)
			{
				CinemachineExtantion.SwitchPriority(pointVirtualCamera);

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