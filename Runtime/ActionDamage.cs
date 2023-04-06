using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionDamage : Action
    {
        private Healthable _healthable;
        private bool _isDeath;

        protected new void Awake()
        {
            base.Awake();

            _healthable = mainTransform.gameObject.AddThisComponent<Healthable>();
        }

        public void TakeDamage(float value, string name = "Damage")
        {
            _healthable.TakeDamage(value);
        }

        public void TakeDeath(string name = "Death")
        {
            _isDeath = true;
            Type = ActionType.Irreversible;
            animatorable.Play(name);
        }

        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop()
        {

        }

        public override void FixedLoop()
        {

        }

        public override void Exit() => movable.FreezAll();
    }
}