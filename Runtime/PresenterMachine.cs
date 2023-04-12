using System.Collections.Generic;
using UnityEngine;


namespace AssemblyActorCore
{
    public enum PresenterType { Controller, Interaction, Forced, Irreversible, Required };

    public class PresenterMachine : MonoBehaviour
    {
        public string GetName => _currentPresenter == null ? "None" : _currentPresenter.gameObject.name + " - " + _currentPresenter.Name;
        public bool IsEmpty => _currentPresenter == null;

        private Presenter _currentPresenter = null;
        private List<Presenter> _presenters = new List<Presenter>();
        private List<Activator> _activators = new List<Activator>();

        private void Start()
        {
            foreach (Presenter presenter in GetComponentsInChildren<Presenter>()) _presenters.Add(presenter);
            foreach (Activator activator in GetComponentsInChildren<Activator>()) _activators.Add(activator);
        }

        private void Update()
        {
            foreach (Activator item in _activators)
            {
                if (item.gameObject != _currentPresenter?.gameObject)
                {
                    item.UpdateActivate();
                }
            }

            _currentPresenter?.UpdateLoop();
        }

        private void FixedUpdate()
        {
            _currentPresenter?.FixedLoop();
        }

        // Check the Actions list for a ready-made Action
        // If you find an equal GameObject Name, execute this Action
        // If you don't find an equal Action, create a new one
        public void TryToActivate(GameObject objectPresenter)
        {
            foreach (Presenter item in _presenters)
            {
                if (objectPresenter.name == item.gameObject.name)
                {
                    InvokeActivate(item.gameObject);

                    return;
                }
            }

            CreateAction(objectPresenter);
        }

        // If the Action is empty, we can activate any other type
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool _isController => _currentPresenter == null ? false : _currentPresenter.Type == PresenterType.Controller;
        private bool _isIrreversible => _currentPresenter == null ? false : _currentPresenter.Type == PresenterType.Irreversible;
        private bool _isReady(Presenter presenter)
        {
            if (IsEmpty == true)
            {
                return true;
            }
            else
            {
                if (_currentPresenter.Type != PresenterType.Required)
                {
                    if (presenter.Type == PresenterType.Required)
                    {
                        return true;
                    }

                    if (_isIrreversible == false)
                    {
                        return _isController ? presenter.Type != PresenterType.Controller : presenter.Type == PresenterType.Forced;
                    }
                }
            }

            return false;
        }
        private void InvokeActivate(GameObject objectPresenter)
        {
            Presenter presenter = objectPresenter.GetComponent<Presenter>();

            if (_isReady(presenter))
            {
                _currentPresenter?.Exit();
                _currentPresenter = presenter;
                _currentPresenter.Enter();

                Debug.Log(GetName);
            }
        }

        private void CreateAction(GameObject objectPresenter)
        {
            GameObject instantiatePresenter = Instantiate(objectPresenter, transform);
            instantiatePresenter.name = objectPresenter.name;
            instantiatePresenter.transform.localPosition = Vector3.zero;
            instantiatePresenter.transform.localRotation = Quaternion.identity;
            _presenters.Add(instantiatePresenter.GetComponent<Presenter>());

            InvokeActivate(instantiatePresenter);
        }

        // At the end of the Action, remove it from the Actor
        public void Deactivate(GameObject objectPresenter)
        {
            Presenter presenter = objectPresenter.GetComponent<Presenter>();

            if (presenter == _currentPresenter)
            {
                _currentPresenter.Exit();
                _currentPresenter = null;
            }
        }
    }
}