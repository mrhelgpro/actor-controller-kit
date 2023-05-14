namespace Actormachine
{
    public abstract class StateBehaviour : ActorBehaviour
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

        public void UpdateInspector()
        {
            _actor = GetComponentInRoot<Actor>();
            _state = GetComponent<State>();

            StateBehaviour[] stateBehaviours = GetComponents<StateBehaviour>();

            foreach (StateBehaviour stateBehaviour in stateBehaviours) stateBehaviour.Initiation();
        }

        public abstract void Initiation();

        public string StateName => _state.Name;
        public bool ActorIsFree => _actor.IsFree;
        public bool IsCurrentState => _actor.IsCurrentState(_state);
        protected void TryToActivate() => _actor.TryToActivate(_state);
        protected void Deactivate() => _actor.Deactivate(_state);
    }
}