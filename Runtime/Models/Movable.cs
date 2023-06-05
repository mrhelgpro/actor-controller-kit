using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - for speed control. </summary>
    public class Movable : ModelBehaviour
    {
        [Range(1, 5)] public float WalkSpeed = 3.0f;
        [Range(1, 10)] public float RunSpeed = 5.0f;
        [Range(0, 5)] public int JumpHeight = 2;
        [Range(0, 2)] public int ExtraJumps = 0;
        [Range(0, 1)] public float Levitation = 0.25f;
        [Range(0, 2)] public float Gravity = 1f;

        public int JumpCounter(bool grounded, bool jump = false)
        {
            if (grounded)
            {
                _jumpCounter = ExtraJumps;
            }
            else
            {
                if (jump == true)
                {
                    _jumpCounter--;
                }
            }

            return _jumpCounter;
        }

        // Movable Fields
        //private float _speedScale = 1;
        //private float _gravityScale = 1;

        private int _jumpCounter = 0;
        private Vector3 _lerpDirection = Vector3.zero;

        // Return Value
        //public float GetSpeed(float value) => (_speedScale < 0 ? 0 : _speedScale) * value;
        //public float GetGravity() => (_gravityScale < 0 ? 0 : _gravityScale) * Gravity;
        public Vector3 GetVelocity(Vector3 direction, float speed, float deltaTime)
        {
            _lerpDirection = Vector3.Lerp(_lerpDirection, direction, deltaTime);

            return new Vector3(_lerpDirection.x, direction.y, _lerpDirection.z) * speed;
        }

        // Change Value
        //public void ChangeSpeed(float value) => _speedScale += value;
        //public void ChangeGravity(float value) => _gravityScale += value;
    }
}