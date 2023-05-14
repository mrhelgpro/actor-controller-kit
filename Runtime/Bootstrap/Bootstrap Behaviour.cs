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

	public interface IRequireBootstrap<T>
	{

	}

	public interface IRequireComponentInRoot<T>
	{

	}
}