using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
	public class Delegator : MonoBehaviour
	{
		public GameObject Prefab;

		private void Awake()
		{
			if (Prefab == null)
			{
				gameObject.SetActive(false);
			}
		}

		protected void TryToActivate(Actionable actionable) => actionable.TryToActivate(Prefab);
	}
}
