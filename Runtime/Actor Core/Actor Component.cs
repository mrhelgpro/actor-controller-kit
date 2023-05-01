using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class ActorComponent : MonoBehaviour
    {
        public Transform RootTransform { get; private set; }
        public Transform ThisTransform { get; private set; }


        protected void Awake()
        {
            Actor actor = GetComponentInParent<Actor>();

            RootTransform = actor == null ? transform : actor.transform;
            ThisTransform = transform;
        }

        public T RequireComponent<T>() where T : Component
        {
            Actor actor = GetComponentInParent<Actor>();
            
            return actor.gameObject.GetComponent<T>() == null ? actor.gameObject.AddComponent<T>() : actor.gameObject.GetComponent<T>();
        }
    }
}