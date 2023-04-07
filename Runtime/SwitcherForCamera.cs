using UnityEngine;

namespace AssemblyActorCore
{
	public class SwitcherForCamera : DetectedByTrigger
	{
		private ActorCamera _actorCamera;
		private TargetForCamera _targetForCamera;
		private TargetForCamera _playerForCamera;

        private void Awake()
        {
			_actorCamera = FindObjectOfType<ActorCamera>();
			_targetForCamera = GetComponent<TargetForCamera>();

			Tag = "Player";
        }

        public override void OnTargetEnter(GameObject target)
		{
			_playerForCamera = target.GetComponentInChildren<TargetForCamera>();
			_actorCamera.Target = _targetForCamera;
		}

		public override void OnTargetExit(GameObject target)
		{
			_actorCamera.Target = _playerForCamera;
			_playerForCamera = null;
		}
	}
}