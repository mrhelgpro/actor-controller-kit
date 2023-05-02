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

        /// <summary> Returns or creates the required component on the Actor. </summary>
        public T GetComponentInActor<T>() where T : Component
        {
            Actor actor = GetComponentInParent<Actor>();

            if (GetComponentInParent<Actor>())
            {
                return actor.gameObject.GetComponent<T>() == null ? actor.gameObject.AddComponent<T>() : actor.gameObject.GetComponent<T>();
            }

            Debug.LogWarning(gameObject.name + " - is not found <Actor>");
            gameObject.SetActive(false);

            return null;
        }
    }
}