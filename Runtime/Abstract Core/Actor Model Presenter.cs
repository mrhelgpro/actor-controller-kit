using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Model : MonoBehaviour
    {
        public Transform RootTransform { get; private set; }
        public Transform ThisTransform { get; private set; }


        protected void Awake()
        {
            Actor actor = GetComponentInParent<Actor>();

            RootTransform = actor == null ? transform : actor.transform;
            ThisTransform = transform;
        }
    }

    public abstract class Presenter : Model
    {
        public PresenterType Type;
        public string Name = "Presenter";

        protected PresenterMachine presenterMachine;

        protected new void Awake()
        {
            base.Awake();

            presenterMachine = GetComponentInParent<PresenterMachine>();
        }

        protected void TryToActivate() => presenterMachine.TryToActivate(gameObject);

        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();
    }

    public abstract class Activator : Model
    {
        protected PresenterMachine presenterMachine;
        protected Presenter presenter;

        protected new void Awake()
        {
            base.Awake();

            presenter = GetComponent<Presenter>();

            if (presenter == null)
            {
                gameObject.SetActive(false);

                Debug.LogWarning(gameObject.name + " - <Presenter> is not found");
            }
            else
            {
                presenterMachine = GetComponentInParent<PresenterMachine>();
            }
        }

        public abstract void UpdateActivate();

        protected void TryToActivate() => presenterMachine.TryToActivate(gameObject);
        protected void Deactivate() => presenterMachine.Deactivate(gameObject);
        protected bool isCurrentPresenter => presenterMachine.GetPresenter == null ? false : gameObject == presenterMachine.GetPresenter.gameObject;
    }
}