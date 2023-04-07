using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class DamageByTrigger : DetectedByTrigger
    {
		public float Value = 1;

		private List<GameObject> _targets;

		private void Awake() => Layer = LayerMask.GetMask("Damagable");

		public override void OnTargetEnter(GameObject target)
		{
			Debug.Log(target.name + " DAMAGABLE");

			Damagable damagable = target.GetComponent<Damagable>();
			target = damagable.GetRootObject;

			foreach (GameObject gameObject in _targets)
			{
				if (gameObject == target)
				{
					return;
				}
			}

			_targets.Add(target);
			//damagable.TakeDamage(Value);
		}

		public override void OnTargetExit(GameObject target)
		{

		}

        private void OnDisable() => _targets.Clear();
	}
}