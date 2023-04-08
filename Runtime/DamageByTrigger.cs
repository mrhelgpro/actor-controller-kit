using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

				Invoke(nameof(refresh), 1);
			}
		}

		public override void OnTargetExit(Transform target) { }

		private void OnDisable() => refresh();

		private void refresh() => _targets.Clear();
	}

#if UNITY_EDITOR
	[ExecuteInEditMode]
	[CustomEditor(typeof(DamageByTrigger))]
	public class DamageByTriggerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DamageByTrigger myTarget = (DamageByTrigger)target;

			myTarget.Value = EditorGUILayout.FloatField("Value", myTarget.Value);
			myTarget.Tag = EditorGUILayout.TextField("Tag", myTarget.Tag);
		}
	}
#endif
}