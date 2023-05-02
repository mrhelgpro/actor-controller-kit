using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class StatePresenterMachine : MonoBehaviour
    {
        private StatePresenter _currentStatePresenter = null;
        
        private List<StatePresenter> _statePresentersList = new List<StatePresenter>();
        private List<Activator> _activatorsList = new List<Activator>();

        private void Start()
        {
            foreach (StatePresenter statePresenter in GetComponentsInChildren<StatePresenter>()) _statePresentersList.Add(statePresenter);
            foreach (Activator activator in GetComponentsInChildren<Activator>()) _activatorsList.Add(activator);
        }
        private void Update()
        {
            foreach (Activator activator in _activatorsList) activator.CheckLoop();

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
            foreach (StatePresenter statePresenter in _statePresentersList)
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
        private bool _isController => _currentStatePresenter == null ? false : _currentStatePresenter.Type == PresenterType.Controller;
        private bool _isIrreversible => _currentStatePresenter == null ? false : _currentStatePresenter.Type == PresenterType.Irreversible;
        private bool _isReady(StatePresenter statePresenter)
        {
            if (IsEmpty == true)
            {
                return true;
            }
            else
            {
                if (_currentStatePresenter.Type != PresenterType.Required)
                {
                    if (statePresenter.Type == PresenterType.Required)
                    {
                        return true;
                    }

                    if (_isIrreversible == false)
                    {
                        return _isController ? true : statePresenter.Type == PresenterType.Forced;
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
            _statePresentersList.Add(instantiateStatePresenter.GetComponent<StatePresenter>());

            InvokeActivate(instantiateStatePresenter);
        }
    }

    [RequireComponent(typeof(StatePresenter))]
    [RequireComponent(typeof(Activator))]
    public abstract class Presenter : ActorComponent
    {
        protected StatePresenter statePresenter;
        protected StatePresenterMachine stateMachine;
        public string Name => statePresenter.Name;

        private new void Awake()
        {
            base.Awake();

            statePresenter = GetComponentInParent<StatePresenter>();
            stateMachine = GetComponentInParent<StatePresenterMachine>();

            Initiation();
        }

        /// <summary> Called once during Awake. Use "GetComponentInActor". </summary>
        protected abstract void Initiation();

        /// <summary> Called once when "Presenter" starts running. </summary>
        public virtual void Enter() { }

        /// <summary> Called every time after "Enter" when using Update. </summary>
        public abstract void UpdateLoop();

        /// <summary> Called once when "Presenter" stops running. </summary>
        public virtual void Exit() { }
    }
}