using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Model
    {
        public Transform RootTransform { get; private set; }
        public Transform ThisTransform { get; private set; }

        public virtual void Initialization(Transform transform)
        {
            Actor actor = transform.GetComponentInParent<Actor>();

            RootTransform = actor == null ? transform : actor.transform;
            ThisTransform = transform;
        }
    }

    public abstract class ModelComponent : MonoBehaviour
    {
        public Transform RootTransform { get; private set; }
        public Transform ThisTransform { get; private set; }


        protected void Awake()
        {
            Actor actor = GetComponentInParent<Actor>();

            RootTransform = actor == null ? transform : actor.transform;
            ThisTransform = transform;
        }
    }

    public abstract class Component : MonoBehaviour
    {
        public Transform RootTransform { get; private set; }
        public Transform ThisTransform { get; private set; }


        protected void Awake()
        {
            Actor actor = GetComponentInParent<Actor>();

            RootTransform = actor == null ? transform : actor.transform;
            ThisTransform = transform;
        }
    }
}