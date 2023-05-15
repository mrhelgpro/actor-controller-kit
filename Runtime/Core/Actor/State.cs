using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public enum StateType { Controller, Interaction, Forced, Irreversible, Required };

    /// <summary> State to update Presenters. </summary>
    public sealed class State : ActorBehaviour
    {
        public string Name = "Controller";
        public StateType Type;

        private List<Presenter> _presenters = new List<Presenter>();

        protected override void Initiation() 
        {
            Bootstrap.Create<BootstrapActor>();

            _presenters = new List<Presenter>();

            foreach (Presenter controller in GetComponentsInChildren<Presenter>()) _presenters.Add(controller);
        }

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

    public abstract class StateBehaviour : ActorBehaviour, IInitInEditMode
    {
        private Actor _actor;
        private State _state;

        public new void InitInEditMode() => Awake();

        protected new void Awake()
        {
            base.Awake();

            _actor = GetComponentInRoot<Actor>();
            _state = GetComponent<State>();
        }

        public string StateName => _state.Name;
        public bool ActorIsFree => _actor.IsFree;
        public bool IsCurrentState => _actor.IsCurrentState(_state);
        protected void TryToActivate() => _actor.TryToActivate(_state);
        protected void Deactivate() => _actor.Deactivate(_state);
    }

    /// <summary> 
    /// Do not use the standard Update, FixedUpdate methods,
    /// instead use the overrides UpdateLoop, FixedUpdateLoop methods
    /// </summary>
    [RequireComponent(typeof(State))]
    [RequireComponent(typeof(Activator))]
    public abstract class Presenter : StateBehaviour
    {
        /// <summary> Called once when "Presenter" starts running. </summary>
        public virtual void Enter() { }

        /// <summary> Called after Enter in Update. </summary>
        public virtual void UpdateLoop() { }

        /// <summary> Called after Enter in FixedUpdate. </summary>
        public virtual void FixedUpdateLoop() { }

        /// <summary> Called once when "Presenter" stops running. </summary>
        public virtual void Exit() { }
    }
}