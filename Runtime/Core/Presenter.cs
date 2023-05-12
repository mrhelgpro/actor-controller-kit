using UnityEngine;

namespace Actormachine
{
    /// <summary> 
    /// Do not use the standard Update, FixedUpdate methods,
    /// instead use the overrides UpdateLoop, FixedUpdateLoop methods
    /// </summary>
    [RequireComponent(typeof(State))]
    [RequireComponent(typeof(Activator))]
    public abstract class Presenter : ActorBehaviour
    {
        private Actor _actor;
        private State _state;

        public string Name => _state.Name;

        private new void Awake()
        {
            base.Awake();

            _actor = GetComponentInRoot<Actor>();
            _state = GetComponentInParent<State>();

            Initiation();
        }

        protected void Deactivate() => _actor.Deactivate(_state);

        /// <summary> Called once during Awake. Use "GetComponentInRoot". </summary>
        public abstract void Initiation();

        /// <summary> Called once when "Presenter" starts running. </summary>
        public virtual void Enter() { }

        /// <summary> Called after Enter in Update. </summary>
        public abstract void UpdateLoop();

        /// <summary> Called after Enter in FixedUpdate. </summary>
        public virtual void FixedUpdateLoop() { }

        /// <summary> Called once when "Presenter" stops running. </summary>
        public virtual void Exit() { }
    }
}