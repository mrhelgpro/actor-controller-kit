using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public enum StateType { Controller, Interaction, Forced, Irreversible, Required };

    /// <summary> State to update Presenters. </summary>
    public sealed class State : ActorBehaviour
    {
        public string Name = "Controller";
        public StateType Type = StateType.Controller;

        private List<Activator> _activators = new List<Activator>();
        private List<Deactivator> _deactivators = new List<Deactivator>();
        private List<Presenter> _presenters = new List<Presenter>();

        private Actor _actor;

        public override void Initiate() 
        {
            Bootstrap.Create<BootstrapActor>();

            _actor = AddComponentInRoot<Actor>();

            _activators = new List<Activator>();
            _deactivators = new List<Deactivator>();
            _presenters = new List<Presenter>();

            foreach (Activator activator in GetComponents<Activator>()) _activators.Add(activator);
            foreach (Deactivator deactivator in GetComponents<Deactivator>()) _deactivators.Add(deactivator);
            foreach (Presenter controller in GetComponents<Presenter>()) _presenters.Add(controller); 
        }

        public bool IsCurrentState => _actor.IsCurrentState(this);

        // Activator Loop
        public void ActivatorLoop()
        {
            if (_activators.Count > 0)
            {
                foreach (Activator activator in _activators) activator.UpdateLoop();

                return;
            }

            if (_actor.IsFree)
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
        }
    }

    /// <summary> 
    /// Do not use the standard Update, FixedUpdate methods,
    /// instead use the overrides UpdateLoop, FixedUpdateLoop methods
    /// </summary>
    [RequireComponent(typeof(State))]
    public abstract class Presenter : ActorBehaviour
    {
        private State _state;

        private new void Awake()
        {
            base.Awake();

            _state = GetComponent<State>();
        }

        public string StateName => _state.Name;

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
    public abstract class Activator : ActorBehaviour
    {
        private bool _currentAvailable = false;
        private bool _previousAvailable = false;

        private State _state;

        private new void Awake()
        {
            base.Awake();

            _state = GetComponent<State>();
        }

        // State Methods
        public bool IsAvailable => _currentAvailable;

        public void SetAvailable(bool value)
        {
            _currentAvailable = value;

            if (_previousAvailable != _currentAvailable)
            {
                if (_currentAvailable == true)
                {
                    _state.Activate();
                }
            }

            _previousAvailable = _currentAvailable;
        }

        /// <summary> Called in Update to check to activate Presenter. </summary>
        public abstract void UpdateLoop();
    }

    /// <summary> To deactivate the Presenters. </summary>
    public abstract class Deactivator : ActorBehaviour
    {
        private State _state;

        private new void Awake()
        {
            base.Awake();

            _state = GetComponent<State>();
        }

        public void Deactivate() => _state.Deactivate();

        /// <summary> Called in Update to check to deactivate Presenter. </summary>
        public abstract void UpdateLoop();
    }
}