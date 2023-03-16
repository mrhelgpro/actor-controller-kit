using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class ActionBehaviour : MonoBehaviour
    {
        public ActionType Type;
        public string Name = "Action";

        protected Input input => _inputable.Input;

        protected Actionable actionable;
        protected Animatorable animatorable;
        protected Movable movable;

        protected GameObject myGameObject;
        protected Transform mainTransform;

        private Inputable _inputable;

        protected void Awake()
        {
            myGameObject = gameObject;

            _inputable = GetComponentInParent<Inputable>();
            actionable = GetComponentInParent<Actionable>();
            animatorable = GetComponentInParent<Animatorable>();
            movable = GetComponentInParent<Movable>();

            actionable.AddActionToPool(myGameObject);

            mainTransform = actionable.transform;

            Initialization();
        }

        protected abstract void Initialization();
        public abstract void WaitLoop();
        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();
    }
}