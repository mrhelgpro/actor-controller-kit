using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Model : MonoBehaviour
    {
        public Transform GetRoot => mainTransform;

        protected Transform myTransform;
        protected Transform mainTransform;

        protected void Awake()
        {
            Actor actor = GetComponentInParent<Actor>();

            mainTransform = actor == null ? transform : actor.transform;
            myTransform = transform;
        }
    }
}
