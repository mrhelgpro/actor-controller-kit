using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Activator : Model
    {
        protected Input input => _inputable.Input;

        protected Actionable actionable;

        private Inputable _inputable;

        private new void Awake()
        {
            base.Awake();

            actionable = GetComponentInParent<Actionable>();
            _inputable = GetComponentInParent<Inputable>();
        }

        protected void TryToActivate() => actionable.TryToActivate(myTransform);

        public abstract void UpdateActivate();
    }
}