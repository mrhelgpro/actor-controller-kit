using UnityEngine;

namespace AssemblyActorCore
{
    [RequireComponent(typeof(StatePresenter))]
    [RequireComponent(typeof(Activator))]
    public abstract class Presenter : ActorComponent
    {
        protected StatePresenterMachine stateMachine;
        private StatePresenter _statePresenter;

        public string Name => _statePresenter.Name;

        private new void Awake()
        {
            base.Awake();

            stateMachine = GetComponentInRoot<StatePresenterMachine>();
            _statePresenter = GetComponentInParent<StatePresenter>();

            Initiation();
        }

        /// <summary> Called once during Awake. Use "GetComponentInRoot". </summary>
        protected abstract void Initiation();

        /// <summary> Called once when "Presenter" starts running. </summary>
        public virtual void Enter() { }

        /// <summary> Called every time after "Enter" when using Update. </summary>
        public abstract void UpdateLoop();

        /// <summary> Called once when "Presenter" stops running. </summary>
        public virtual void Exit() { }
    }
}