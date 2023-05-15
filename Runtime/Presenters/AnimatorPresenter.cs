using UnityEngine;

namespace Actormachine
{
    public class AnimatorPresenter : Presenter
    {
        public RuntimeAnimatorController AnimatorController;

        public float Fade = 0.025f;

        // Model Components
        private Animatorable _animatorable;

        // Presenter Methods
        protected override void Initiation()
        {
            _animatorable = GetComponentInRoot<Animatorable>();

            // Check Required Component
            if (AnimatorController == null)
            {
                Debug.LogWarning(gameObject.name + " - You need to add an AnimatorController");
            }
        }

        public override void Enter() => _animatorable.Enter(AnimatorController, StateName, Fade);

        public override void Exit() => _animatorable.Exit();
    }
}