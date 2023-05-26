using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public enum StatePriority { Default, Prepare, Action };

    public sealed class State : ActorBehaviour
    {
        public bool IsActive { get; private set; } = false;

        public StatePriority Priority = StatePriority.Default;

        // State interfaces
        private List<IInactiveState> _inactiveStates = new List<IInactiveState>();
        private List<IEnterState> _enterStates = new List<IEnterState>();
        private List<IActiveState> _activeStates = new List<IActiveState>();
        private List<IFixedActiveState> _fixedActiveStates = new List<IFixedActiveState>();
        private List<IExitState> _exitStates = new List<IExitState>();
        private List<IDisableState> _disableStates = new List<IDisableState>();

        private Actor _actor = null;

        // Actor Methods
        public void Activate() => _actor.Activate(this);
        public void Deactivate() => _actor.Deactivate(this);

        // State Methods
        public void OnEnableState()
        {
            // Clear lists
            _inactiveStates.Clear();
            _enterStates.Clear();
            _activeStates.Clear();
            _fixedActiveStates.Clear();
            _exitStates.Clear();
            _disableStates.Clear();

            _actor = GetComponentInParent<Actor>();

            _actor.Add(this);

            // Add interfaces
            foreach (IInactiveState state in GetComponents<IInactiveState>()) _inactiveStates.Add(state);
            foreach (IEnterState state in GetComponents<IEnterState>()) _enterStates.Add(state);
            foreach (IActiveState state in GetComponents<IActiveState>()) _activeStates.Add(state);
            foreach (IFixedActiveState state in GetComponents<IFixedActiveState>()) _fixedActiveStates.Add(state);
            foreach (IExitState state in GetComponents<IExitState>()) _exitStates.Add(state);
            foreach (IDisableState state in GetComponents<IDisableState>()) _disableStates.Add(state);

            // Enable interfaces
            foreach (IEnableState state in GetComponents<IEnableState>()) state.OnEnableState();
        }
        public void OnInactiveState()
        {
            foreach (IInactiveState state in _inactiveStates) state.OnInactiveState();
        }
        public void OnEnterState()
        {
            IsActive = true;

            foreach (IEnterState state in _enterStates) state.OnEnterState();
        }
        public void OnActiveState()
        {
            foreach (IActiveState state in _activeStates) state.OnActiveState();
        }
        public void OnFixedActiveState()
        {
            foreach (IFixedActiveState state in _fixedActiveStates) state.OnFixedActiveState();
        }
        public void OnExitState()
        {
            foreach (IExitState state in _exitStates) state.OnExitState();

            IsActive = false;
        }
        public void OnDisableState()
        {
            foreach (IDisableState state in _disableStates) state.OnDisableState();

            _actor.Remove(this);
            _actor = null;

            IsActive = false;
        }
    }

    [RequireComponent(typeof(State))]
    public abstract class StateBehaviour : ActorBehaviour
    {
        protected State state;

        private new void Awake()
        {
            base.Awake();

            state = GetComponent<State>();
        }
    }

    /// <summary> To activate if State is not activated. </summary>
    public abstract class Activator : StateBehaviour, IEnterState
    {
        private bool _currentAvailable = false;
        public bool IsAvailable() => _currentAvailable;

        public void OnEnterState()
        {
            _currentAvailable = true;
        }

        public void TryActive(bool value)
        {
            if (value != _currentAvailable)
            {
                _currentAvailable = value;

                if (_currentAvailable == true)
                {
                    checkAllAvailable();
                }
            }
        }

        private void checkAllAvailable()
        {
            int amountOfAvailable = 0;

            Activator[] activators = GetComponents<Activator>();

            foreach (Activator activator in activators) amountOfAvailable += activator.IsAvailable() ? 1 : 0;

            if (amountOfAvailable == activators.Length) state.Activate();
        }
    }

    /// <summary> To deactivate if State is active. </summary>
    public abstract class Deactivator : StateBehaviour
    {
        public void Deactivate() => state.Deactivate();
    }

    // State Interfaces
    public interface IEnableState
    {
        public void OnEnableState();
    }

    public interface IInactiveState
    {
        public void OnInactiveState();
    }

    public interface IEnterState
    {
        public void OnEnterState();
    }

    public interface IActiveState
    {
        public void OnActiveState();
    }

    public interface IFixedActiveState
    {
        public void OnFixedActiveState();
    }

    public interface IExitState
    {
        public void OnExitState();
    }

    public interface IDisableState
    {
        public void OnDisableState();
    }

    /// <summary> Mark for the ability to interact. </summary>
    public interface IInteractable
    {
        /// <summary> Checks if the object is available for interaction. </summary>
        public bool IsAvailable(Transform rootTransform);
    }
}