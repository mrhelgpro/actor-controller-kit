using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Activator : MonoBehaviour
    {
        protected Input input => _inputable.Input;

        protected GameObject myGameObject;
        protected Actionable actionable;

        private Inputable _inputable;

        private void Awake()
        {
            myGameObject = gameObject;

            actionable = GetComponentInParent<Actionable>();
            _inputable = GetComponentInParent<Inputable>();
        }

        protected void TryToActivate() => actionable.TryToActivate(myGameObject);

        public abstract void UpdateActivate();
    }
}