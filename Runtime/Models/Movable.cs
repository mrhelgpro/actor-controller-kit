using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - for speed control. </summary>
    public class Movable : ModelBehaviour
    {
        // Movable Fields
        private float _speedScale = 1;
        private float _gravityScale = 1;

        private Vector3 _lerpDirection = Vector3.zero;
        private float _velocityMagnitude = 0;

        // Return Value
        public float GetSpeed(float value) => (_speedScale < 0 ? 0 : _speedScale) * value;
        public float GetGravity(float value) => (_gravityScale < 0 ? 0 : _gravityScale) * value;
        public Vector3 GetVelocity(Vector3 direction, float speed, float deltaTime)
        {
            float currentSpeed = GetSpeed(speed);

            _lerpDirection = Vector3.Lerp(_lerpDirection, direction, deltaTime);

            Vector3 velocity = new Vector3(_lerpDirection.x, direction.y, _lerpDirection.z) * currentSpeed;

            _velocityMagnitude = velocity.magnitude;

            return velocity;
        }
        public float GetVelocityMagnitude => _velocityMagnitude;

        // Change Value
        public void ChangeSpeed(float value) => _speedScale += value;
        public void ChangeGravity(float value) => _gravityScale += value;
    }
}