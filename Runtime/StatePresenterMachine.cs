using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public enum ControllerType { Controller, Interaction, Forced, Irreversible, Required };

    public class StatePresenterMachine : MonoBehaviour
    {
        private StatePresenter _currentStatePresenter = null;
        private List<StatePresenter> _listStatePresenters = new List<StatePresenter>();
        
        private List<Activator> _activators = new List<Activator>();

        private void Start()
        {
            foreach (StatePresenter statePresenter in GetComponentsInChildren<StatePresenter>()) _listStatePresenters.Add(statePresenter);
            foreach (Activator activator in GetComponentsInChildren<Activator>()) _activators.Add(activator);
        }
        private void Update()
        {
            foreach (Activator activator in _activators) activator.UpdateActivate();

            _currentStatePresenter?.UpdateLoop();
        }

        public StatePresenter GetCurrentState => _currentStatePresenter;
        public string GetName => _currentStatePresenter == null ? "None" : _currentStatePresenter.gameObject.name + " - " + _currentStatePresenter.Name;
        public bool IsEmpty => _currentStatePresenter == null;

        // Check the Actions list for a ready-made Action
        // If you find an equal GameObject Name, execute this Action
        // If you don't find an equal Action, create a new one
        public void TryToActivate(GameObject objectStatePresenter)
        {
            foreach (StatePresenter statePresenter in _listStatePresenters)
            {
                if (objectStatePresenter.name == statePresenter.gameObject.name)
                {
                    InvokeActivate(statePresenter.gameObject);

                    return;
                }
            }

            CreateAction(objectStatePresenter);
        }

        // At the end of the Action, remove it from the Actor
        public void Deactivate(GameObject objectStatePresenter)
        {
            StatePresenter statePresenter = objectStatePresenter.GetComponent<StatePresenter>();

            if (statePresenter == _currentStatePresenter)
            {
                _currentStatePresenter.Exit();
                _currentStatePresenter = null;
            }
        }

        // If the Action is empty, we can activate any other type
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool _isController => _currentStatePresenter == null ? false : _currentStatePresenter.Type == ControllerType.Controller;
        private bool _isIrreversible => _currentStatePresenter == null ? false : _currentStatePresenter.Type == ControllerType.Irreversible;
        private bool _isReady(StatePresenter statePresenter)
        {
            if (IsEmpty == true)
            {
                return true;
            }
            else
            {
                if (_currentStatePresenter.Type != ControllerType.Required)
                {
                    if (statePresenter.Type == ControllerType.Required)
                    {
                        return true;
                    }

                    if (_isIrreversible == false)
                    {
                        return _isController ? true : statePresenter.Type == ControllerType.Forced;
                    }
                }
            }

            return false;
        }
        private void InvokeActivate(GameObject objectStatePresenter)
        {
            StatePresenter statePresenter = objectStatePresenter.GetComponent<StatePresenter>();

            if (_isReady(statePresenter))
            {
                _currentStatePresenter?.Exit();
                _currentStatePresenter = statePresenter;
                _currentStatePresenter.Enter();
            }
        }

        private void CreateAction(GameObject objectStatePresenter)
        {
            GameObject instantiateStatePresenter = Instantiate(objectStatePresenter, transform);
            instantiateStatePresenter.name = objectStatePresenter.name;
            instantiateStatePresenter.transform.localPosition = Vector3.zero;
            instantiateStatePresenter.transform.localRotation = Quaternion.identity;
            _listStatePresenters.Add(instantiateStatePresenter.GetComponent<StatePresenter>());

            InvokeActivate(instantiateStatePresenter);
        }
    }

    [RequireComponent(typeof(StatePresenter))]
    public abstract class Presenter : ActorComponent
    {
        protected StatePresenter statePresenter;
        protected StatePresenterMachine stateMachine;
        public string Name => statePresenter.Name;

        protected new void Awake()
        {
            base.Awake();

            statePresenter = GetComponentInParent<StatePresenter>();
            stateMachine = GetComponentInParent<StatePresenterMachine>();
        }
        public virtual void Enter() { }
        public abstract void UpdateLoop();
        public virtual void Exit() { }
    }
}