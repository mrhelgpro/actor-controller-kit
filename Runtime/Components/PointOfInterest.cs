using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace AssemblyActorCore
{
	public class PointOfInterest : DetectedByTrigger
	{
		[Range(20, 80)] public int FieldOfView = 60;
		[Range(-180, 180)] public float Horizontal;
		[Range(-90, 90)] public float Vertical;
		[Range(1, 20)] public float Distance = 10;
		[Range(0.1f, 5)] public float EnterTime = 1.0f;
		[Range(0.1f, 2)] public float ExitTime = 1.0f;
		public bool ReturnToBack = false;

		private CinemachineBrain _cinemachineBrain;
		private ActorVirtualCamera _actorVirtualCamera;
		private CinemachineVirtualCamera _playerVirtualCamera;
		private CinemachineVirtualCamera _pointVirtualCamera;

		private void Awake()
        {
			_actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();
			_playerVirtualCamera = _actorVirtualCamera.GetComponent<CinemachineVirtualCamera>();
			_pointVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
			_cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();

			_pointVirtualCamera.Priority = 0;
			TargetTag = "Player";
		}

        public override void OnTargetEnter(Transform target)
		{
			_actorVirtualCamera.IsLock = true;
			_cinemachineBrain.m_DefaultBlend.m_Time = EnterTime;
			_pointVirtualCamera.Priority = 20;
		}

		public override void OnTargetExit(Transform target)
		{
			_actorVirtualCamera.IsLock = false;

			if (ReturnToBack == true)
			{
				_playerVirtualCamera.Follow.transform.localEulerAngles = Vector3.zero;

				_actorVirtualCamera.Parameters.OrbitHorizontal = _playerVirtualCamera.Follow.transform.eulerAngles.y;
				_actorVirtualCamera.Parameters.OrbitVertical = _playerVirtualCamera.Follow.transform.eulerAngles.x;
			}
			else
			{
				_actorVirtualCamera.Parameters.OrbitHorizontal = _pointVirtualCamera.transform.eulerAngles.y;
				_actorVirtualCamera.Parameters.OrbitVertical = _pointVirtualCamera.transform.eulerAngles.x;
			}

			_cinemachineBrain.m_DefaultBlend.m_Time = ExitTime;
			_pointVirtualCamera.Priority = 0;
		}
	}

#if UNITY_EDITOR
	[ExecuteInEditMode]
	[CustomEditor(typeof(PointOfInterest))]
	public class PointOfInterestEditor : ModelEditor
	{
		public override void OnInspectorGUI()
		{
			CameraBootstrap bootstrap = FindAnyObjectByType<CameraBootstrap>();

			if (bootstrap == null)
			{
				DrawModelBox("<CameraBootstrap> is not found", BoxStyle.Error);

				return;
			}

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
#endif
}