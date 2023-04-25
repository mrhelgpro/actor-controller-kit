using UnityEngine;

namespace AssemblyActorCore
{
	public class SwitcherForCamera : DetectedByTrigger
	{
		private ActorCamera _actorCamera;
		private Followable _targetForCamera;
		private Followable _playerForCamera;

        private void Awake()
        {
			_actorCamera = FindObjectOfType<ActorCamera>();
			_targetForCamera = GetComponent<Followable>();

			Tag = "Player";
        }

        public override void OnTargetEnter(Transform target)
		{
			_playerForCamera = target.GetComponentInChildren<Followable>();
			_actorCamera.Target = _targetForCamera;
		}

		public override void OnTargetExit(Transform target)
		{
			_actorCamera.Target = _playerForCamera;
			_playerForCamera = null;
		}
	}
}