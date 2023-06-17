using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - for speed control. </summary>
    [AddComponentMenu("Actormachine/Model/Movable")]
    public class Movable : Model
    {
        [Range(1, 5)] public float WalkSpeed = 3.0f;
        [Range(1, 10)] public float RunSpeed = 5.0f;
        [Range(0, 2)] public float Gravity = 1f;
        [Range(0, 5)] public int JumpHeight = 2;
        [Range(0, 2)] public int ExtraJumps = 0;
        [Range(0, 1)] public float Levitation = 0.25f;
        public int JumpCounter = 0;

        // Buffer Fields
        private Vector3 _lerpDirection = Vector3.zero;

        // Return Value
        public Vector3 GetVelocity(Vector3 direction, float speed, float deltaTime)
        {
            _lerpDirection = Vector3.Lerp(_lerpDirection, direction, deltaTime);

            return new Vector3(_lerpDirection.x, direction.y, _lerpDirection.z) * speed;
        }
    }
}