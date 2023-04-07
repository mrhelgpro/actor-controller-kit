using UnityEngine;

namespace AssemblyActorCore
{
    public enum ActionType { Controller, Interaction, Forced, Irreversible, Required };

    public abstract class Action : Model
    {
        public ActionType Type;
        public string Name = "Action";

        protected Input input => _inputable.Input;

        protected Actionable actionable;
        protected Animatorable animatorable;
        protected Rotable rotable;
        protected Movable movable;
        protected Positionable positionable;

        private Inputable _inputable;

        protected new void Awake()
        {
            base.Awake();

            _inputable = GetComponentInParent<Inputable>();
            actionable = GetComponentInParent<Actionable>();
            animatorable = GetComponentInParent<Animatorable>();
            rotable = GetComponentInParent<Rotable>();
            movable = GetComponentInParent<Movable>();
            positionable = GetComponentInParent<Positionable>();
        }

        protected void TryToActivate() => actionable.TryToActivate(myTransform);

        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();       
    }
}