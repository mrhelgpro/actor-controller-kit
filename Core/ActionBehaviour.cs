using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class ActionBehaviour : MonoBehaviour
    {
        public string Name = "Action";
        public new ActionType GetType => type;
        protected ActionType type;

        protected Inputable inputable;
        protected Actionable actionable;
        protected Animatorable animatorable;
        protected Movable movable;

        protected GameObject myGameObject;
        protected Transform mainTransform;

        protected void Awake()
        {
            myGameObject = gameObject;

            inputable = GetComponentInParent<Inputable>();
            actionable = GetComponentInParent<Actionable>();
            animatorable = GetComponentInParent<Animatorable>();
            movable = GetComponentInParent<Movable>();

            actionable.AddActionToPool(myGameObject);

            inputable.Input += InputHandler;

            mainTransform = actionable.transform;

            Initialization();
        }

        public virtual void InputHandler() { }

        protected abstract void Initialization();
        public abstract void WaitLoop();
        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();

        private void OnDestroy()
        {
            inputable.Input -= InputHandler;
        }
    }
}