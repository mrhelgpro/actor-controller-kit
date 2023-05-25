using UnityEngine;

namespace Actormachine
{
    public enum PlayMode { BySpeed, ByTime };

    public class AnimatorPresenter : Presenter
    {
        public string PlayName = "Move";

        public RuntimeAnimatorController Controller;

        public PlayMode PlayMode = PlayMode.BySpeed;
        public float Duration = 1;

        private float _timer = 0;
        private Animatorable _animatorable;

        // Presenter Methods
        public override void Enter()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root    
            _animatorable = AddComponentInRoot<Animatorable>();

            _animatorable.Enter(Controller);
        }

        public override void UpdateLoop()
        {
            string playName = _animatorable.Grounded ? PlayName : "Fall";
            float speed = PlayMode == PlayMode.BySpeed ? 1 : 1 / Duration;
            _animatorable.Play(playName, speed);

            if (PlayMode == PlayMode.ByTime)
            {
                _timer += Time.deltaTime;

                if (_timer >= Duration)
                {
                    state.Deactivate();
                }
            }
        }

        public override void Exit()
        {
            _timer = 0;
            _animatorable.Stop();
            _animatorable.Exit();
        }
    }
}