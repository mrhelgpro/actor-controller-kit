using UnityEngine;
using UnityEditor;

namespace Actormachine
{
    /// <summary> Checks or creates all required components. </summary>
    public abstract class Bootstrap : MonoBehaviour, IInitInEditMode
    {
        public void InitInEditMode() => Awake();

        private void Awake() => Initiation();

        /// <summary> In Play Mode it is called once when Awake, in Edit Mode it is called constantly as an Update. </summary>
        protected abstract void Initiation();

        /// <summary> Create the required <see cref="Bootstrap"/> to get all the dependent components. </summary>
        public static T Create<T>() where T : Bootstrap
        {
            T currentBootstrap = FindAnyObjectByType<T>();

            if (currentBootstrap == null)
            {
                Bootstrap anyBootstrap = FindAnyObjectByType<Bootstrap>();
                Transform parentBootstrap;

                if (anyBootstrap == null)
                {
                    parentBootstrap = new GameObject().transform;
                }
                else
                {
                    parentBootstrap = anyBootstrap.transform;
                }

                currentBootstrap = parentBootstrap.gameObject.AddComponent<T>();

                currentBootstrap.transform.SetAsFirstSibling();
                EditorGUIUtility.PingObject(currentBootstrap.gameObject);
            }

            currentBootstrap.transform.name = "Bootstrap";
            currentBootstrap.transform.parent = null;
            currentBootstrap.transform.localScale = Vector3.one;
            currentBootstrap.transform.position = Vector3.zero;
            currentBootstrap.transform.rotation = Quaternion.identity;

            return currentBootstrap;
        }
    }

    public static class BootstrapExtantion
    {
        /// <summary> 
        /// Returns "true" if there is one instance of the class on the Scene, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool IsSingleInstanceOnScene<T>() where T : Component
        {
            return IsSingleInstance<T>(GameObject.FindObjectsOfType<T>());
        }

        /// <summary> 
        /// Returns "true" if there is one instance of the class on gameObject, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool IsSingleInstanceOnObject<T>(this GameObject gameObject) where T : Component
        {
            return IsSingleInstance<T>(gameObject.GetComponents<T>());
        }

        /// <summary> 
        /// Returns "true" if there is one instance of the class in children, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool IsSingleInstanceInChildren<T>(this GameObject gameObject) where T : Component
        {
            return IsSingleInstance<T>(gameObject.GetComponentsInChildren<T>());
        }

        /// <summary> 
        /// Returns "true" if there is one instance, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool IsSingleInstance<T>(T[] instances) where T : Component
        {
            if (instances.Length > 1)
            {
                Debug.LogWarning("<" + typeof(T).ToString() + "> should be a single Instance");

                return false;
            }

            return instances.Length == 1;
        }
    }

    public interface IInitInEditMode
    {
        /// <summary> In Edit Mode it is called constantly as an Update. </summary>
        public void InitInEditMode();

    }
}