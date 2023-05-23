using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public enum StatePriority { Free, Prepare, Action };

    /// <summary> State to update Presenters. </summary>
    public sealed class State : ActorBehaviour
    {
        public string Name = "State";
        public StatePriority Priority = StatePriority.Free;

        private List<Activator> _activators = new List<Activator>();
        private List<Deactivator> _deactivators = new List<Deactivator>();
        private List<Presenter> _presenters = new List<Presenter>();

        private bool _isEnable = false;
        private bool _isActive = false;

        private Actor _actor;

        public bool IsEnabled => _isEnable;
        public bool IsActive => _isActive;

        public void Enable()
        {
            _isEnable = true;

            _actor = GetComponentInParent<Actor>();

            _activators = new List<Activator>();
            _deactivators = new List<Deactivator>();
            _presenters = new List<Presenter>();

            foreach (Activator activator in GetComponents<Activator>())
            {
                _activators.Add(activator);
                activator.Enable();
            }

            foreach (Deactivator deactivator in GetComponents<Deactivator>())
            {
                _deactivators.Add(deactivator);
                deactivator.Enable();
            }

            foreach (Presenter controller in GetComponents<Presenter>()) _presenters.Add(controller);
        }

        public void Disanable()
        {
            _isEnable = false;
        }

        // Activator Loop
        public void ActivatorLoop()
        {
            if (_activators.Count > 0)
            {
                foreach (Activator activator in _activators) activator.UpdateLoop();

                return;
            }

            if (Priority == StatePriority.Free)
            {
                _actor.Activate(this);
            }
        }

        public void DeactivatorLoop()
        {
            foreach (Deactivator deactivator in _deactivators) deactivator.UpdateLoop();
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

        public void Deactivate() => _actor.Deactivate(this);

        // Presenter Loop
        public void Enter()
        {
            _isActive = true;

            foreach (Presenter controller in _presenters) controller.Enter();
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

            _isActive = false;
        }
    }

    public abstract class StateBehaviour : ActorBehaviour
    {
        protected State state;

        private new void Awake()
        {
            base.Awake();

            state = GetComponent<State>();
        }

        public abstract void Enable();
    }

    /// <summary> 
    /// Do not use the standard Update, FixedUpdate methods,
    /// instead use the overrides UpdateLoop, FixedUpdateLoop methods
    /// </summary>
    [RequireComponent(typeof(State))]
    public abstract class Presenter : ActorBehaviour
    {
        protected State state;                 // FIXED IT
        private new void Awake()               // FIXED IT
        {
            base.Awake();                      // FIXED IT
            state = GetComponent<State>();     // FIXED IT
        }
        public string StateName => state.Name; // FIXED IT

        /// <summary> Called once when "Presenter" starts running. </summary>
        public virtual void Enter() { }

        /// <summary> Called after Enter in Update. </summary>
        public virtual void UpdateLoop() { }

        /// <summary> Called after Enter in FixedUpdate. </summary>
        public virtual void FixedUpdateLoop() { }

        /// <summary> Called once when "Presenter" stops running. </summary>
        public virtual void Exit() { }
    }

    /// <summary> To activate the Presenters. </summary>
    public abstract class Activator : StateBehaviour
    {
        private bool _currentAvailable = false;
        private bool _previousAvailable = false;

        // State Methods
        public bool IsAvailable => _currentAvailable;

        public void SetActive(bool value)
        {
            _currentAvailable = value;

            if (_previousAvailable != _currentAvailable)
            {
                if (_currentAvailable == true)
                {
                    state.Activate();
                }
            }

            _previousAvailable = _currentAvailable;
        }

        /// <summary> Called in Update to check to activate Presenter. </summary>
        public abstract void UpdateLoop();
    }

    /// <summary> To deactivate the Presenters. </summary>
    public abstract class Deactivator : StateBehaviour
    {
        public void Deactivate() => state.Deactivate();

        /// <summary> Called in Update to check to deactivate Presenter. </summary>
        public abstract void UpdateLoop();
    }
}