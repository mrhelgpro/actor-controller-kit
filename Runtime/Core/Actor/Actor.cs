using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Core/Actor")]
    /// <summary> Actor is as a State Machine that controls states. </summary>
    public sealed class Actor : ActorBehaviour
    {
        public string Name = "Actor";

        // State Machine
        private State _currentState = null;
        private State _defaultState = null;
        private List<State> _currentStates = new List<State>();
        private List<State> _addingToStates = new List<State>();
        private List<State> _removeStates = new List<State>();

        // Default methods
        private void Start()
        {
            // Enable States
            foreach (State state in GetComponentsInChildren<State>()) Add(state);
        }

        private void FixedUpdate()
        {
            // FixedUpdates State after Enter
            _currentState?.OnFixedActiveState();
        }

        private void Update()
        {
            // Updates the OnInactiveState() if State is not activated
            foreach (State state in _currentStates)
            {
                if (IsCurrentState(state) == false) state.OnInactiveState();
            }

            // Updates Deactivator if State is active
            _currentState?.OnActiveState();
        }

        private void LateUpdate()
        {
            // Add and Remove States
            foreach (State state in _addingToStates)
            {
                _currentStates.Add(state);

                state.OnEnableState();
            }

            foreach (State state in _removeStates)
            {
                _currentStates.Remove(state);
                state.OnDisableState();

                // Deactivate default State
                clearDefaultState(state);
            }

            _addingToStates.Clear();
            _removeStates.Clear();

            // Set default State if current State is null
            if (_currentState == null)
            {
                if (_defaultState == null)
                {
                    foreach (State state in _currentStates)
                    {
                        if (state.Priority == StatePriority.Default)
                        {
                            Activate(state);
                            return;
                        }
                    }
                }
                else
                {
                    Activate(_defaultState);
                }
            }
        }

        // State machine methods
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
                _removeStates.Add(state);
            }
        }

        /// <summary> 
        /// Activates the state by priority. 
        /// "Default" - can only be activated if current priority is Default.
        /// "Prepare" - can be activated if current priority is Default or Prepare.
        /// "Action" - can be activated by any other priority.
        /// </summary>
        public void Activate(State state)
        {
            if (isExists(state) == true)
            {
                if (isReady(state))
                {
                    // Deactivate previous State, and call Exit
                    State exitState = _currentState;
                    _currentState = null;
                    exitState?.OnExitState();

                    // Activate next State, and call Enter
                    _currentState = state;
                    _currentState.OnEnterState();

                    // Set State as Default
                    if (_currentState != null)
                    {
                        if (_currentState.Priority == StatePriority.Default)
                        {
                            _defaultState = _currentState;
                        }
                    }
                }
            }
        }

        /// <summary> Deactivates State, and default State . </summary>
        public void Deactivate(State state)
        {
            if (state == _currentState)
            {
                // Deactivate default State
                clearDefaultState(state);

                // Deactivate current State, and call Exit
                State exitState = _currentState;
                _currentState = null;
                exitState?.OnExitState();
            }
        }

        public List<State> GetStatesList => _currentStates;    
        public bool IsCurrentState(State state) => _currentState == null ? false : state == _currentState;
        private bool isExists(State state)
        {
            foreach (State stateInList in _currentStates)
            {
                if (stateInList == state)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isReady(State state)
        {
            // If the Actor is free, we can activate any other State
            if (_currentState == null)
            {
                return true;
            }
            else
            {
                // Default can only be activated if current priority is Default
                if (state.Priority == StatePriority.Default)
                {
                    if (_currentState.Priority == StatePriority.Default)
                    {
                        return true;
                    }

                    return false;
                }

                // Prepare can be activated if current priority is Default or Prepare
                if (state.Priority == StatePriority.Prepare)
                {
                    if (_currentState.Priority != StatePriority.Action)
                    {
                        return true;
                    }
                }

                // Action can be activated by any other priority
                if (state.Priority == StatePriority.Action)
                {
                    return true;
                }
            }

            return false;
        }

        private void clearDefaultState(State state)
        {
            if (state == _defaultState)
            {
                _defaultState = null;
            }
        }
    }
}