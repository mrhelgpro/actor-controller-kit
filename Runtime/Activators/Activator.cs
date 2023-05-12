using UnityEngine;

namespace Actormachine
{
    /// <summary> To activate the Presenters. </summary>
    public class Activator : ActorBehaviour
    {
        private Actor _actor;
        private State _state;

        private new void Awake()
        {
            base.Awake();

            _actor = GetComponentInRoot<Actor>();
            _state = GetComponent<State>();

            Initiation();
        }

        /// <summary> Called in Update to check to activate Presenter. </summary>
        public virtual void CheckLoop()
        {
            if (_actor.IsFree)
            {
                TryToActivate();
            }
        }

        public bool IsCurrentState => _actor.IsCurrentState(_state);

        /// <summary> Called once during Awake. Use "GetComponentInRoot". </summary>
        protected virtual void Initiation() { }
        protected void TryToActivate() => _actor.TryToActivate(_state);
        protected void Deactivate() => _actor.Deactivate(_state);
    }
}