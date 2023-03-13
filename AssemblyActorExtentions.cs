using UnityEngine;

namespace AssemblyActorCore
{
    public static class Extension
    {
        // Finds the required Component on <Actor> gets or instantiates
        public static T AddThisComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null) Object.DestroyImmediate(gameObject.GetComponent<T>());
        }
    }
}