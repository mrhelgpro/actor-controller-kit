using UnityEngine;

namespace Actormachine
{
    public abstract class ModelBehaviour : ActorBehaviour
    {
        public Transform RootTransform { get; private set; }

        protected new void Awake()
        {
            base.Awake();

            RootTransform = FindRootTransform;
        }
    }
}