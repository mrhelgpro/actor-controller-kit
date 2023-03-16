using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Activator : MonoBehaviour
    {
        protected Input input => _inputable.Input;
        private Inputable _inputable;
        private Actionable _actionable;
        private GameObject _myGameObject;    

        private void Awake()
        {
            _myGameObject = gameObject;
            _actionable = GetComponentInParent<Actionable>();
            _inputable = GetComponentInParent<Inputable>();
        }

        public void Activate() => _actionable.Activate(_myGameObject);

        protected abstract void UpdateActivate();
    }
}