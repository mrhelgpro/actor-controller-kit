using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - for speed control. </summary>
    public class Movable : ActorBehaviour
    {
        // Movable Fields
        private float _speedScale = 1;
        private float _gravityScale = 1;

        // Return Value
        public float GetSpeed(float value) => (_speedScale < 0 ? 0 : _speedScale) * value;
        public float GetGravity(float value) => (_gravityScale < 0 ? 0 : _gravityScale) * value;

        // Change Value
        public void ChangeSpeed(float value) => _speedScale += value;
        public void ChangeGravity(float value) => _gravityScale += value;
    }
}