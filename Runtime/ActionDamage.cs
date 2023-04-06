namespace AssemblyActorCore
{
    public class ActionDamage : ActionInteraction
    {
        private Healthable _healthable;

        protected new void Awake()
        {
            base.Awake();

            _healthable = mainTransform.gameObject.AddThisComponent<Healthable>();
        }

        private void OnEnable() => _healthable.EventDamage += DamageHandler;

        protected virtual void DamageHandler()
        {
            if (_healthable.IsDead == false)
            {
                TryToActivate();
            }
            else
            {
                TakeKill();
            }
        }

        public void TakeKill(string name = "Death")
        {
            Name = name;
            Type = ActionType.Required;
            _healthable.TakeKill();

            TryToActivate();
        }

        public override void UpdateLoop()
        {
            if (_healthable.IsDead == false)
            {
                base.UpdateLoop();
            }
        }

        public override void Exit()
        {
            movable.FreezAll();
            Type = ActionType.Forced;
            Name = "Damage";
        }

        private void OnDisable() => _healthable.EventDamage -= DamageHandler;
    }
}