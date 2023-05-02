using UnityEngine;

namespace AssemblyActorCore
{
    [RequireComponent(typeof(StatePresenter))]
    public abstract class Activator : ActorComponent
    {
        protected StatePresenterMachine stateMachine;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (GetComponent<Presenter>() == null)
            {
                Debug.LogWarning(gameObject.name + " - Activator: is not found <Presenter>");
            }

            if (GetComponentInParent<StatePresenterMachine>() == null)
            {
                Debug.LogWarning(gameObject.name + " - Activator: is not found <StatePresenterMachine>");
            }
        }
#endif

        private new void Awake()
        {
            base.Awake();

            stateMachine = GetComponentInParent<StatePresenterMachine>();

            Initiation();
        }

        /// <summary> Called in Update to check to activate Presenter. </summary>
        public abstract void CheckLoop();

        /// <summary> Called once during Awake. Use "GetComponentInActor". </summary>
        protected virtual void Initiation() { }

        protected void TryToActivate() => stateMachine.TryToActivate(gameObject);
        protected void Deactivate() => stateMachine.Deactivate(gameObject);
        protected bool isCurrentState => stateMachine.GetCurrentState == null ? false : gameObject == stateMachine.GetCurrentState.gameObject;
    }
}