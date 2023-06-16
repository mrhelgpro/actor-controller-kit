using UnityEngine;

namespace Actormachine
{
    /// <summary> All classes that are part of the Actor must inherit from this class. </summary>
    public abstract class ActormachineComponentBase : PlayComponentBase
    {
        public Transform ThisTransform { get; private set; }

        protected void Awake()
        {
            ThisTransform = transform;
        }

        /// <summary> Add or Get the component on the Root. </summary>
        public T AddComponentInRoot<T>() where T : Component
        {
            GameObject root = FindRootTransform.gameObject;

            return root.GetComponentInChildren<T>() == null ? root.AddComponent<T>() : root.GetComponentInChildren<T>();
        }

        /// <summary> Finds the highest object in the hierarchy that contains "ActorBehaviour" and uses it as a marker for the root object. </summary>
        public Transform FindRootTransform
        {
            get 
            {
                ActormachineComponentBase actormachineComponentBase = this;

                Transform rootTransform = transform;

                while (rootTransform.parent != null)
                {
                    rootTransform = rootTransform.parent;

                    if (rootTransform.GetComponent<ActormachineComponentBase>())
                    {
                        actormachineComponentBase = rootTransform.GetComponent<ActormachineComponentBase>();
                    }
                }

                return actormachineComponentBase.transform;
            }
        }
    }
}