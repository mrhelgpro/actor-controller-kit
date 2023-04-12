namespace AssemblyActorCore
{
    public abstract class Activator : Model
    {
        protected PresenterMachine presenterMachine;

        protected new void Awake()
        {
            base.Awake();

            presenterMachine = GetComponentInParent<PresenterMachine>();
        }

        protected void TryToActivate() => presenterMachine.TryToActivate(myTransform.gameObject);

        public abstract void UpdateActivate();
    }
}