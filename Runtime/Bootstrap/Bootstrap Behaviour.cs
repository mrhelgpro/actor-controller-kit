using UnityEngine;
using UnityEditor;

namespace Actormachine
{
	public abstract class Bootstrap : MonoBehaviour
	{
		private void Awake() => Initiation();

		/// <summary> In Play Mode it is called once when Awake, in Edit Mode it is called constantly as an Update. </summary>
		public abstract void Initiation();
	}

/*
#if UNITY_EDITOR
	[ExecuteInEditMode]
	public abstract class BootstrapEditor : MonoBehaviour
	{
		private void OnEnable() => EditorApplication.update += UpdateInEditMode;

		private void OnDisable() => EditorApplication.update -= UpdateInEditMode;
		private void UpdateInEditMode()
		{
			if (Application.isPlaying == false)
			{
				UpdateBootstrap();
			}
		}

		public abstract void UpdateBootstrap();
	}
#endif
*/
}