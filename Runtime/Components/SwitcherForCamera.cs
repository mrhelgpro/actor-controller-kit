using UnityEngine;

namespace AssemblyActorCore
{
	public class SwitcherForCamera : DetectedByTrigger
	{
		private ActorCamera _actorCamera;
		private Followable _targetEnter;
		private Followable _targetExit;

        private void Awake()
        {
			_actorCamera = FindObjectOfType<ActorCamera>();
			_targetEnter = GetComponent<Followable>();

			Tag = "Player";
        }

        public override void OnTargetEnter(Transform target)
		{
			_targetExit = target.GetComponentInChildren<Followable>();
			_actorCamera.Target = _targetEnter;
		}

		public override void OnTargetExit(Transform target)
		{
			_targetExit.Parametres.Orbit.Horizontal = _targetEnter.Parametres.Orbit.Horizontal;
			_targetExit.Parametres.Orbit.Vertical = _targetEnter.Parametres.Orbit.Vertical;
			_actorCamera.Target = _targetExit;
			_targetExit = null;
		}
	}
}