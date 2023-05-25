using UnityEngine;

namespace Actormachine
{
    public class DeactivatorByTime : Deactivator
    {
        public float Duration = 1;

        private float _timer = 0;

        public override void UpdateLoop()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration)
            {
                Deactivate();
            }
        }

        public override void Exit()
        {
            _timer = 0;
        }
    }
}