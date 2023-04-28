namespace AssemblyActorCore
{
    public abstract class Controller : ModelComponent
    {
        public ControllerType Type;
        public string Name = "Controller";

        protected ControllerMachine controllerMachine;

        protected new void Awake()
        {
            base.Awake();

            controllerMachine = GetComponentInParent<ControllerMachine>();
        }

        protected void TryToActivate() => controllerMachine.TryToActivate(gameObject);

        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();
    }
}    