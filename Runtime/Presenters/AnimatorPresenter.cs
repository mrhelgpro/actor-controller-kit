using UnityEngine;

namespace Actormachine
{
    public class AnimatorPresenter : Presenter
    {
        public RuntimeAnimatorController AnimatorController;

        private Animatorable _animatorable;

        // Presenter Methods
        public override void Initiation()
        {
            _animatorable = GetComponentInRoot<Animatorable>();

            if (AnimatorController == null)
            {
                Debug.LogWarning(gameObject.name + " - You need to add an AnimatorController");
            }
        }

        public override void Enter() => _animatorable.Enter(AnimatorController);

        public override void Exit() => _animatorable.Exit();
    }
}