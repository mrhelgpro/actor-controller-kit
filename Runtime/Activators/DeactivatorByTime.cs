using UnityEngine;

namespace Actormachine
{
    public class DeactivatorByTime : Deactivator, IActiveState, IExitState
    {
        public float Duration = 1;

        private float _timer = 0;

        public void OnActiveState()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration)
            {
                Deactivate();
            }
        }

        public void OnExitState()
        {
            _timer = 0;
        }
    }
}