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
        private List<State> _currentStates = new List<State>();
        private List<State> _addingToStates = new List<State>();
        private List<State> _removeStates = new List<State>();

        private void Start()
        {
            foreach (State state in GetComponentsInChildren<State>())
            {
                _currentStates.Add(state);
                state.Enable();
            }
        }

        private void FixedUpdate()
        {
            _currentState?.FixedUpdateLoop();
        }

        private void Update()
        {
            foreach (State state in _currentStates)
            {
                if (IsCurrentState(state) == false) state.ActivatorLoop();
            }

            _currentState?.UpdateLoop();
            _currentState?.DeactivatorLoop();
        }

        private void LateUpdate()
        {
            foreach (State item in _addingToStates) _currentStates.Add(item);
            foreach (State item in _removeStates) _currentStates.Remove(item);

            _addingToStates.Clear();
            _removeStates.Clear();
        }

        public void Add(State state)
        {
            if (isExists(state) == false)
            {
                _addingToStates.Add(state);
            }
        }

        public void Remove(State state)
        {
            if (isExists(state) == true)
            {
                _removeStates.Remove(state);
            }
        }

        /// <summary> 
        /// Check the Actions list for a ready-made Action
        /// If you find an equal GameObject Name, execute this Action
        /// If you don't find an equal Action, create a new one.
        /// </summary>
        public void Activate(State state)
        {
            if (isExists(state) == true)
            {
                if (isReady(state))
                {
                    _currentState?.Exit();
                    _currentState = state;
                    _currentState.Enter();
                }
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

        public List<State> GetStatesList => _currentStates;
        public bool IsCurrentState(State state) => _currentState == null ? false : state == _currentState;

        // If the Action is free, we can activate any other state
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool isReady(State state)
        {
            if (_currentState == null)
            {
                return true;
            }
            else
            {
                if (state.Priority == StatePriority.Free)
                {
                    return false;
                }

                if (state.Priority == StatePriority.Prepare)
                {
                    if (_currentState.Priority != StatePriority.Action)
                    {
                        return true;
                    }
                }

                if (state.Priority == StatePriority.Action)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isExists(State checkedState)
        {
            foreach (State state in _currentStates)
            {
                if (state == checkedState)
                {
                    return true;
                }
            }

            return false;
        }
    }
}