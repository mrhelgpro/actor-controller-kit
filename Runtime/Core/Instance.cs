using UnityEngine;

namespace Actormachine
{
    public static class Instance
    {
        public static T AddRequiredComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null) Object.DestroyImmediate(gameObject.GetComponent<T>());
        }

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
}