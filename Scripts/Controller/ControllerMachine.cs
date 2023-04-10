using System.Collections.Generic;
using UnityEngine;


namespace AssemblyActorCore
{
    public enum ControllerType { Controller, Interaction, Forced, Irreversible, Required };

    public class ControllerMachine : MonoBehaviour
    {
        public string GetName => _currentController == null ? "None" : _currentController.gameObject.name + " - " + _currentController.Name;
        public bool IsEmpty => _currentController == null;

        private Controller _currentController = null;
        private List<Controller> _controllers = new List<Controller>();
        private List<Activator> _activators = new List<Activator>();

        private void Awake()
        {
            foreach (Controller controller in GetComponentsInChildren<Controller>()) _controllers.Add(controller);
            foreach (Activator activator in GetComponentsInChildren<Activator>()) _activators.Add(activator);
        }

        private void Update()
        {
            foreach (Activator item in _activators)
            {
                if (item.gameObject != _currentController?.gameObject)
                {
                    item.UpdateActivate();
                }
            }

            _currentController?.UpdateLoop();
        }

        private void FixedUpdate()
        {
            _currentController?.FixedLoop();
        }

        // Check the Actions list for a ready-made Action
        // If you find an equal GameObject Name, execute this Action
        // If you don't find an equal Action, create a new one
        public void TryToActivate(GameObject objectController)
        {
            foreach (Controller item in _controllers)
            {
                if (objectController.name == item.gameObject.name)
                {
                    InvokeActivate(item.gameObject);

                    return;
                }
            }

            CreateAction(objectController);
        }

        // If the Action is empty, we can activate any other type
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool _isController => _currentController == null ? false : _currentController.Type == ControllerType.Controller;
        private bool _isIrreversible => _currentController == null ? false : _currentController.Type == ControllerType.Irreversible;
        private bool _isReady(Controller controller)
        {
            if (IsEmpty == true)
            {
                return true;
            }
            else
            {
                if (_currentController.Type != ControllerType.Required)
                {
                    if (controller.Type == ControllerType.Required)
                    {
                        return true;
                    }

                    if (_isIrreversible == false)
                    {
                        return _isController ? controller.Type != ControllerType.Controller : controller.Type == ControllerType.Forced;
                    }
                }
            }

            return false;
        }
        private void InvokeActivate(GameObject objectController)
        {
            Controller controller = objectController.GetComponent<Controller>();

            if (_isReady(controller))
            {
                _currentController?.Exit();
                _currentController = controller;
                _currentController.Enter();

                Debug.Log(GetName);
            }
        }

        private void CreateAction(GameObject objectController)
        {
            GameObject instantiateController = Instantiate(objectController, transform);
            instantiateController.name = objectController.name;
            instantiateController.transform.localPosition = Vector3.zero;
            instantiateController.transform.localRotation = Quaternion.identity;
            _controllers.Add(instantiateController.GetComponent<Controller>());

            InvokeActivate(instantiateController);
        }

        // At the end of the Action, remove it from the Actor
        public void Deactivate(GameObject objectController)
        {
            Controller controller = objectController.GetComponent<Controller>();

            if (controller == _currentController)
            {
                _currentController.Exit();
                _currentController = null;
            }
        }
    }

    public abstract class Controller : MonoBehaviour
    {
        public ControllerType Type;
        public string Name = "Controller";

        protected ControllerMachine controllerMachine;

        protected void Awake() => controllerMachine = GetComponentInParent<ControllerMachine>();

        protected void TryToActivate() => controllerMachine.TryToActivate(gameObject);

        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();
    }
}