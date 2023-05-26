using UnityEngine;

namespace Actormachine
{
    public enum PlayMode { BySpeed, ByTime };

    public class AnimatorPresenter : StateBehaviour, IEnterState, IActiveState, IExitState
    {
        public string PlayName = "Move";

        public RuntimeAnimatorController Controller;

        public PlayMode PlayMode = PlayMode.BySpeed;
        public float Duration = 1;

        private float _timer = 0;

        private State _state;
        private Animatorable _animatorable;

        // Presenter Methods
        public void OnEnterState()
        {
            _state = GetComponent<State>();

            // Add or Get comppnent in the Root
            _animatorable = AddComponentInRoot<Animatorable>();

            _animatorable.Enter(Controller);
        }

        public void OnActiveState()
        {
            string playName = _animatorable.Grounded ? PlayName : "Fall";
            float speed = PlayMode == PlayMode.BySpeed ? 1 : 1 / Duration;
            _animatorable.Play(playName, speed);

            if (PlayMode == PlayMode.ByTime)
            {
                _timer += Time.deltaTime;

                if (_timer >= Duration)
                {
                    _state.Deactivate();
                }
            }
        }

        public void OnExitState()
        {
            _timer = 0;
            _animatorable.Stop();
            _animatorable.Exit();
        }
    }
}