using UnityEngine;

namespace Actormachine
{
    /// <summary> All classes that are part of the Actor must inherit from this class. </summary>
    public abstract class ActorBehaviour : MonoBehaviour
    {
        public Transform RootTransform { get; private set; }
        public Transform ThisTransform { get; private set; }

        protected void Awake()
        {
            RootTransform = FindRootTransform;
            ThisTransform = transform;

            Initiation();
        }

        /// <summary> In Play Mode it is called once when Awake, in Edit Mode it is called constantly as an Update. </summary>
        public virtual void Initiation() { }

        /// <summary> Returns or creates the required component on the Actor. </summary>
        public T GetComponentInRoot<T>() where T : Component
        {
            GameObject root = FindRootTransform.gameObject;

            return root.GetComponentInChildren<T>() == null ? root.AddComponent<T>() : root.GetComponentInChildren<T>();
        }

        /// <summary> Finds the highest object in the hierarchy that contains "ActorBehaviour" and uses it as a marker for the root object. </summary>
        public Transform FindRootTransform
        {
            get 
            {
                ActorBehaviour actorBehaviour = this;

                Transform rootTransform = transform;

                while (rootTransform.parent != null)
                {
                    rootTransform = rootTransform.parent;

                    if (rootTransform.GetComponent<ActorBehaviour>())
                    {
                        actorBehaviour = rootTransform.GetComponent<ActorBehaviour>();
                    }
                }

                return actorBehaviour.transform;
            }
        }
    }

    public static class ActorBehaviourExtention
    {
        public static T AddRequiredComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null) Object.DestroyImmediate(gameObject.GetComponent<T>());
        }
    }
}