using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(0, 1f)] public float Gravity = 1;

        public bool IsFall = false;
        public bool IsJump = false;

        protected Transform mainTransform;

        protected float getSpeedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getAcceleration => Acceleration > 0 ? Acceleration + 1 : 1;

        protected void Awake() => mainTransform = transform;

        public abstract void FreezAll();
        public abstract void FreezRotation();
        public abstract void MoveToDirection(Vector3 direction, float speed, bool isGrounded = false);
        public abstract void Jump(float force);
        public void MoveToPosition(Vector3 direction, float speed) => mainTransform.position += direction.normalized * speed * Time.fixedDeltaTime;
    }
}