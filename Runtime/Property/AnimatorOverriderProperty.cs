using UnityEngine;

namespace Actormachine
{
    public enum PlayMode { Default, Override };
    public enum LayerMode { Default, Full, Top };
    public enum TimeMode { Parameters, Timer };

    [AddComponentMenu("Actormachine/Property/AnimatorOverrider Property")]
    public class AnimatorOverriderProperty : Property
    {
        public AnimatorOverrideController OverrideController;

        public PlayMode PlayMode = PlayMode.Default;
        public LayerMode LayerMode = LayerMode.Default;
        public TimeMode TimeMode = TimeMode.Parameters;
        public float Duration = 1;

        private float _timer = 0;

        private Animatorable _animatorable;

        // Property Methods
        public override void OnEnterState()
        {
            // Add or Get comppnent in the Root
            _animatorable = AddComponentInRoot<Animatorable>();

            float speed = TimeMode == TimeMode.Parameters ? 1 : 1 / Duration;

            _animatorable.Enter(OverrideController, speed);

            if (LayerMode == LayerMode.Full)
            {
                _animatorable.SetLayerWeight(1, 1);
                _animatorable.SetLayerWeight(2, 0);
            }
            else if (LayerMode == LayerMode.Top)
            {
                _animatorable.SetLayerWeight(2, 1);
                _animatorable.SetLayerWeight(1, 0);
            }
            else
            {
                _animatorable.SetLayerWeight(1, 0);
                _animatorable.SetLayerWeight(2, 0);
            }
        }

        public override void OnActiveState()
        {
            if (TimeMode == TimeMode.Timer)
            {
                _timer += Time.deltaTime;

                if (_timer >= Duration)
                {
                    actor.Deactivate(state);
                }
            }
        }

        public override void OnExitState()
        {
            _timer = 0;

            _animatorable.Exit();
        }
    }
}