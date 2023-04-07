using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class DamageByTrigger : DetectedByTrigger
    {
		public float Value = 1;

		private List<Transform> _targets = new List<Transform>();

		private void Awake() => Layer = LayerMask.GetMask("Damagable");

		public override void OnTargetEnter(Transform target)
		{
			Damagable damagable = target.GetComponent<Damagable>();

			if (damagable == null)
			{
				return;
			}
			else
			{
				target = damagable.GetRoot;

				foreach (Transform transform in _targets)
				{
					if (transform == target)
					{
						return;
					}
				}

				_targets.Add(target);
				damagable.TakeDamage(Value);
			}
		}

		public override void OnTargetExit(Transform target)
		{

		}

        private void OnDisable() => _targets.Clear();
	}
}