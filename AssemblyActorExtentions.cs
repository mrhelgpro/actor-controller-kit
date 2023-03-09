using UnityEngine;

namespace AssemblyActorCore
{
    public static class Extension
    {
        // Finds the required Component on <Actor> gets or instantiates
        public static T GetModel<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            gameObject = gameObject.GetComponentInParent<Actor>().gameObject;

            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }
    }
}