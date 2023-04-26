using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Activator : ModelComponent
    {
        protected ControllerMachine controllerMachine;
        protected Controller controller;

        protected new void Awake()
        {
            base.Awake();

            controller = GetComponent<Controller>();

            if (controller == null)
            {
                gameObject.SetActive(false);

                Debug.LogWarning(gameObject.name + " - <Controller> is not found");
            }
            else
            {
                controllerMachine = GetComponentInParent<ControllerMachine>();
            }
        }

        public abstract void UpdateActivate();

        protected void TryToActivate() => controllerMachine.TryToActivate(gameObject);
        protected void Deactivate() => controllerMachine.Deactivate(gameObject);
        protected bool isCurrentController => controllerMachine.GetController == null ? false : gameObject == controllerMachine.GetController.gameObject;
    }
}