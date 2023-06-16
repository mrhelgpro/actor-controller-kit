using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Core/Actor")]
    /// <summary> Actor is as a State Machine that controls states. </summary>
    public sealed class Actor : ActormachineComponentBase
    {
        public string Name = "Actor";

        // State Machine
        private State _currentState = null;
        private State _nextState = null;
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

        protected override void GameplayFixedUpdate()
        {
            // FixedUpdates State after Enter
            _currentState?.OnFixedActiveState();
        }

        protected override void GameplayUpdate()
        {
            // Updates the OnInactiveState() if State is not activated
            foreach (State state in _currentStates)
            {
                if (IsCurrentState(state) == false) state.OnInactiveState();
            }

            // Updates Deactivator if State is active
            _currentState?.OnActiveState();
        }

        protected override void GameplayLateUpdate()
        {
            // Add States
            foreach (State state in _addingToStates)
            {
                _currentStates.Add(state);

                state.OnEnableState();
            }

            _addingToStates.Clear();

            // Remove States
            foreach (State state in _removeStates)
            {
                _currentStates.Remove(state);
                state.OnDisableState();

                // Deactivate default State
                clearDefaultState(state);
            }

            _removeStates.Clear();

            // Set next State
            if (_nextState)
            {
                activeState(_nextState);
                return;
            }

            // Set default State if current State is null
            if (_currentState == null)
            {
                if (_defaultState)
                {
                    activeState(_defaultState);
                    return;
                }

                foreach (State state in _currentStates)
                {
                    if (state.Priority == StatePriority.Default)
                    {
                        activeState(state);
                        return;
                    }
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
                    _nextState = state;    
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
                deactiveState();
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

        private void activeState(State state)
        {
            // Deactivate previous State, and call Exit
            deactiveState();

            // Activate next State, and call Enter
            _currentState = state;
            _currentState.OnEnterState();

            // Set State as Default
            setDefaultState();

            _nextState = null;
        }

        private void setDefaultState()
        {
            if (_currentState != null)
            {
                if (_currentState.Priority == StatePriority.Default)
                {
                    _defaultState = _currentState;
                }
            }
        }

        private void clearDefaultState(State state)
        {
            if (state == _defaultState)
            {
                _defaultState = null;
            }
        }

        private void deactiveState()
        {
            // Deactivate previous State, and call Exit
            State exitState = _currentState;
            _currentState = null;
            exitState?.OnExitState();
        }
    }
}