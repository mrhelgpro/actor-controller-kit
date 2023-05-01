using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Activator : ActorComponent
    {
        protected StatePresenterMachine stateMachine;
        protected Presenter presenter;

        protected new void Awake()
        {
            base.Awake();

            presenter = GetComponent<Presenter>();

            if (presenter == null)
            {
                gameObject.SetActive(false);

                Debug.LogWarning(gameObject.name + " - Activator: is not found <Presenter>");
            }
            else
            {
                stateMachine = GetComponentInParent<StatePresenterMachine>();
            }
        }

        public abstract void UpdateActivate();

        protected void TryToActivate() => stateMachine.TryToActivate(gameObject);
        protected void Deactivate() => stateMachine.Deactivate(gameObject);
        protected bool isCurrentState => stateMachine.GetCurrentState == null ? false : gameObject == stateMachine.GetCurrentState.gameObject;
    }
}