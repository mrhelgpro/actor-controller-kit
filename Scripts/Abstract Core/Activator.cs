using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Activator : Model
    {
        protected ControllerMachine controllerMachine;

        protected new void Awake()
        {
            base.Awake();

            controllerMachine = GetComponentInParent<ControllerMachine>();
        }

        protected void TryToActivate() => controllerMachine.TryToActivate(myTransform.gameObject);

        public abstract void UpdateActivate();
    }
}