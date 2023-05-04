using System.Collections.Generic;
using UnityEngine;

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
        public List<StatePresenter> GetStatePresentersList => _statePresentersList;
        public bool IsCurrentStateObject(GameObject checkObject) => _currentStatePresenter == null ? false : checkObject == _currentStatePresenter.gameObject;
        public string GetName => _currentStatePresenter == null ? "None" : _currentStatePresenter.gameObject.name + " - " + _currentStatePresenter.Name;
        public bool IsFree => _currentStatePresenter == null;

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
            if (IsFree == true)
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

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [UnityEditor.CustomEditor(typeof(StatePresenterMachine))]
    public class StatePresenterMachineEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            bool error = false;

            StatePresenterMachine thisTarget = (StatePresenterMachine)target;

            // Check StatePresenter
            StatePresenter statePresenter = thisTarget.gameObject.GetComponentInChildren<StatePresenter>();
            if (statePresenter == null)
            {
                DrawModelBox("<StatePresenter> - is not found", BoxStyle.Error);
                error = true;
            }

            if (error == false)
            {
                DrawModelBox("Update the State Presenter");
            }

            UnityEditor.EditorUtility.SetDirty(thisTarget);
        }
    }
#endif
}