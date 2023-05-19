using System.Collections;
using System.Collections.Generic;
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
                _timer = 0;
            }
        }
    }
}