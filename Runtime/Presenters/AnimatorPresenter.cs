using UnityEngine;

namespace Actormachine
{
    public class AnimatorPresenter : Presenter
    {
        public RuntimeAnimatorController Controller;

        private Animatorable _animatorable;

        // Presenter Methods
        public override void Enter()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root    
            _animatorable = AddComponentInRoot<Animatorable>();

            _animatorable.Enter(Controller);
            _animatorable.Play(StateName);
        }

        public override void Exit() => _animatorable.Exit();
    }
}