using UnityEngine;
using Cinemachine;

namespace Actormachine
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

				_actorVirtualCamera.CurrentParameters.OrbitHorizontal = _playerVirtualCamera.Follow.transform.eulerAngles.y;
				_actorVirtualCamera.CurrentParameters.OrbitVertical = _playerVirtualCamera.Follow.transform.eulerAngles.x;
			}
			else
			{
				_actorVirtualCamera.CurrentParameters.OrbitHorizontal = _pointVirtualCamera.transform.eulerAngles.y;
				_actorVirtualCamera.CurrentParameters.OrbitVertical = _pointVirtualCamera.transform.eulerAngles.x;
			}

			_cinemachineBrain.m_DefaultBlend.m_Time = ExitTime;
			_pointVirtualCamera.Priority = 0;
		}
	}
}