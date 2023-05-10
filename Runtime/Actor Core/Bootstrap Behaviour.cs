using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace AssemblyActorCore
{
	[ExecuteInEditMode]
	public abstract class BootstrapBehaviour : MonoBehaviour
	{
#if UNITY_EDITOR
		private void OnEnable() => EditorApplication.update += UpdateInEditMode;
        private void Awake() => UpdateBootstrap();
		private void OnDisable() => EditorApplication.update -= UpdateInEditMode;	
		private void UpdateInEditMode()
		{
			if (Application.isPlaying == false)
			{
				UpdateBootstrap();
			}
		}

		public abstract void UpdateBootstrap();
#endif
	}
}