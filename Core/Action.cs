using UnityEngine;

namespace AssemblyActorCore
{
    public enum ActionType { Controller, Interaction, Forced, Irreversible, Required };

    public abstract class Action : MonoBehaviour
    {
        public ActionType Type;
        public string Name = "Action";

        protected Input input => _inputable.Input;

        protected Actionable actionable;
        protected Animatorable animatorable;
        protected Rotable rotable;
        protected Movable movable;
        protected Positionable positionable;


        protected GameObject myGameObject;
        protected Transform mainTransform;

        private Inputable _inputable;

        protected void Awake()
        {
            myGameObject = gameObject;

            _inputable = GetComponentInParent<Inputable>();
            actionable = GetComponentInParent<Actionable>();
            animatorable = GetComponentInParent<Animatorable>();
            rotable = GetComponentInParent<Rotable>();
            movable = GetComponentInParent<Movable>();
            positionable = GetComponentInParent<Positionable>();

            mainTransform = actionable.transform;
        }

        protected void TryToActivate() => actionable.TryToActivate(myGameObject);

        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();       
    }
}