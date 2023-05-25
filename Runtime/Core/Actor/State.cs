using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public enum StatePriority { Default, Prepare, Action };

    /// <summary> State to update Presenters. </summary>
    public class State : ActorBehaviour
    {
        public StatePriority Priority = StatePriority.Default;

        private List<Activator> _activators = new List<Activator>();
        private List<Deactivator> _deactivators = new List<Deactivator>();
        private List<Presenter> _presenters = new List<Presenter>();

        private bool _isActive = false;

        private Actor _actor;

        public bool IsActive => _isActive;

        public virtual void Enable()
        {
            _activators = new List<Activator>();
            _deactivators = new List<Deactivator>();
            _presenters = new List<Presenter>();

            _actor = GetComponentInParent<Actor>();

            _actor.Add(this);

            // Add Components
            foreach (Activator activator in GetComponents<Activator>()) _activators.Add(activator);
            foreach (Deactivator deactivator in GetComponents<Deactivator>()) _deactivators.Add(deactivator);
            foreach (Presenter controller in GetComponents<Presenter>()) _presenters.Add(controller);

            // Enable State Components
            foreach (StateComponent stateComponent in GetComponents<StateComponent>()) stateComponent.Enable();
        }

        public void Disable()
        {
            _actor.Remove(this);

            _activators = new List<Activator>();
            _deactivators = new List<Deactivator>();
            _presenters = new List<Presenter>();

            _actor = null;
        }

        // Actor Methods
        public void Activate()
        {
            int amountOfReady = 0;

            foreach (Activator activator in _activators) amountOfReady += activator.IsAvailable ? 1 : 0;

            if (amountOfReady == _activators.Count)
            {
                _actor.Activate(this);
            }
        }

        public void Deactivate()
        {
            _actor.Deactivate(this);
        }

        /// <summary> Updates the Activator if State is not activated. </summary>
        public void ActivatorLoop()
        {
            foreach (Activator activator in _activators) activator.UpdateLoop();
        }

        /// <summary> Updates Deactivator if State is active. </summary>
        public void DeactivatorLoop()
        {
            foreach (Deactivator deactivator in _deactivators) deactivator.UpdateLoop();
        }

        // Presenter Loop
        public void Enter()
        {
            _isActive = true;

            foreach (Presenter controller in _presenters) controller.Enter();

            foreach (Activator activator in _activators)
            {
                activator.SetActive(true);
                activator.Enter();
            } 
        }

        public void UpdateLoop()
        {
            foreach (Presenter controller in _presenters) controller.UpdateLoop();
        }

        public void FixedUpdateLoop()
        {
            foreach (Presenter controller in _presenters) controller.FixedUpdateLoop();
        }

        public void Exit()
        {
            foreach (Presenter controller in _presenters) controller.Exit();
            foreach (Deactivator deactivator in _deactivators) deactivator.Exit();
            
            _isActive = false;
        }
    }

    public abstract class StateComponent : ActorBehaviour
    {
        protected State state;

        private new void Awake()
        {
            base.Awake();

            state = GetComponent<State>();
        }

        public virtual void Enable() { }
    }

    /// <summary> 
    /// Do not use the standard Update, FixedUpdate methods,
    /// instead use the overrides UpdateLoop, FixedUpdateLoop methods
    /// </summary>
    [RequireComponent(typeof(State))]
    public abstract class Presenter : StateComponent
    {
        /// <summary> Called once when "Presenter" starts running. </summary>
        public virtual void Enter() { }

        /// <summary> FixedUpdates State after Enter. </summary>
        public virtual void FixedUpdateLoop() { }

        /// <summary> Updates State after Enter. </summary>
        public virtual void UpdateLoop() { }

        /// <summary> Called once when "Presenter" stops running. </summary>
        public virtual void Exit() { }
    }

    /// <summary> To activate if State is not activated. </summary>
    public abstract class Activator : StateComponent
    {
        private bool _currentAvailable = false;
        public bool IsAvailable => _currentAvailable;

        public void SetActive(bool value)
        {
            if (value != _currentAvailable)
            {
                _currentAvailable = value;

                if (_currentAvailable == true)
                {
                    state.Activate();
                }
            }
        }

        public abstract void UpdateLoop();
        public virtual void Enter() { }
    }

    /// <summary> To deactivate if State is active. </summary>
    public abstract class Deactivator : StateComponent
    {
        public void Deactivate() => state.Deactivate();
        public abstract void UpdateLoop();
        public virtual void Exit() { }
    }
}