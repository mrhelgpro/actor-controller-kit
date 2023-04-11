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

    public abstract class Controller : MonoBehaviour
    {
        public ControllerType Type;
        public string Name = "Controller";

        protected ControllerMachine controllerMachine;

        protected void Awake() => controllerMachine = GetComponentInParent<ControllerMachine>();

        protected void TryToActivate() => controllerMachine.TryToActivate(gameObject);

        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();
    }
}
