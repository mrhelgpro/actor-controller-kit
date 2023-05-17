using UnityEngine;

namespace Actormachine
{
    public class AnimatorPresenter : Presenter
    {
        public RuntimeAnimatorController Controller;

        private Animatorable _animatorable;

        // Presenter Methods
        public override void Initiation()
        {
            _animatorable = GetComponentInRoot<Animatorable>();

            if (Controller == null)
            {
                Debug.LogWarning(gameObject.name + " - You need to add an AnimatorController");
            }
        }

        public override void Enter() => _animatorable.Enter(Controller);

        public override void Exit() => _animatorable.Exit();
    }
}