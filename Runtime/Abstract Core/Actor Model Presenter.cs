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
        protected void TryToActivate() => presenterMachine.TryToActivate(gameObject);

        public abstract void UpdateActivate();
    }
}