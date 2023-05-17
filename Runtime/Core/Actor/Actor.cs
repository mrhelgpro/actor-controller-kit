using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    /// <summary> Actor is as a State Machine that controls states. </summary>
    public sealed class Actor : ActorBehaviour
    {
        public string Name = "Actor";

        // State Machine
        private State _currentState = null;
        private List<State> _statesList = new List<State>();

        private void Start()
        {
            foreach (State state in GetComponentsInChildren<State>()) _statesList.Add(state);

            BootstrapActor.AddActor(this);
        }

        public void UpdateLoop()
        {
            foreach (State state in _statesList)
            {
                if (IsCurrentState(state) == false) state.ActivatorLoop();
            }

            _currentState?.UpdateLoop();
            _currentState?.DeactivatorLoop();
        }

        public void FixedUpdateLoop()
        {
            _currentState?.FixedUpdateLoop();
        }

        /// <summary> Give all child objects the "Actror" layer. </summary>
        public void SetActorLayer(Transform transform)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Actor");

            foreach (Transform child in transform)
            {
                SetActorLayer(child);
            }
        }

        public State GetCurrentState => _currentState;
        public List<State> GetStatesList => _statesList;
        public bool IsCurrentState(State state) => _currentState == null ? false : state == _currentState;
        public string GetName => _currentState == null ? "None" : _currentState.gameObject.name + " - " + _currentState.Name;
        public bool IsFree => _currentState == null;

        /// <summary> 
        /// Check the Actions list for a ready-made Action
        /// If you find an equal GameObject Name, execute this Action
        /// If you don't find an equal Action, create a new one.
        /// </summary>
        public void Activate(State state)
        {
            if (_statesList.Exists(a => a == state))
            {
                InvokeActivate(state);

                return;
            }

            CreateAction(state.gameObject);
        }

        // If the Action is empty, we can activate any other type
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool _isController => _currentState == null ? false : _currentState.Type == StateType.Controller;
        private bool _isIrreversible => _currentState == null ? false : _currentState.Type == StateType.Irreversible;
        private bool _isReady(State state)
        {
            if (IsFree == true)
            {
                return true;
            }
            else
            {
                if (_currentState.Type != StateType.Required)
                {
                    if (state.Type == StateType.Required)
                    {
                        return true;
                    }

                    if (_isIrreversible == false)
                    {
                        return _isController ? true : state.Type == StateType.Forced;
                    }
                }
            }

            return false;
        }
        private void InvokeActivate(State state)
        {
            if (_isReady(state))
            {
                _currentState?.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }

        /// <summary> At the end of the Action, remove it from the Actor. </summary>
        public void Deactivate(State state)
        {
            if (state == _currentState)
            {
                _currentState.Exit();
                _currentState = null;
            }
        }

        private void CreateAction(GameObject objectState)
        {
            GameObject instantiateState = Instantiate(objectState, transform);
            instantiateState.name = objectState.name;
            instantiateState.transform.localPosition = Vector3.zero;
            instantiateState.transform.localRotation = Quaternion.identity;

            State state = instantiateState.GetComponent<State>();
            _statesList.Add(state);

            InvokeActivate(state);
        }
    }
}