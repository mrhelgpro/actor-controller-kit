using UnityEngine;

namespace AssemblyActorCore
{
    public class Activator : ActorComponent
    {
        protected StatePresenterMachine stateMachine;

        private new void Awake()
        {
            base.Awake();

            stateMachine = GetComponentInActor<StatePresenterMachine>();

            Initiation();
        }

        /// <summary> Called in Update to check to activate Presenter. </summary>
        public virtual void CheckLoop()
        {
            if (stateMachine.IsEmpty)
            {
                TryToActivate();
            }
        }

        /// <summary> Called once during Awake. Use "GetComponentInActor". </summary>
        protected virtual void Initiation() { }
        protected void TryToActivate() => stateMachine.TryToActivate(gameObject);
        protected void Deactivate() => stateMachine.Deactivate(gameObject);
        protected bool isCurrentState => stateMachine.GetCurrentState == null ? false : gameObject == stateMachine.GetCurrentState.gameObject;
    }
}