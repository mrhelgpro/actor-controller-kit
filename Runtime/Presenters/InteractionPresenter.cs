using UnityEngine;

namespace Actormachine
{
    public class InteractionPresenter : Presenter
    {
        // Model Components
        private Interactable _interactable;

        // Presenter Methods
        public override void Enter()
        {
            // Get components using "GetComponentInRoot" to create them on <Actor>
            _interactable = AddComponentInRoot<Interactable>();

            _interactable.SetTargetByType<Grabbable>();
            _interactable.Target.parent = ThisTransform;
            _interactable.Target.localPosition = Vector3.zero;
            _interactable.Target.localEulerAngles = Vector3.zero;

            State[] states = _interactable.Target.GetComponentsInChildren<State>();

            foreach (State state in states) state.Enable();
        }

        public override void Exit()
        {
            //_interactable.Target.parent = null;
            _interactable.Target = null;
        }
    }
}