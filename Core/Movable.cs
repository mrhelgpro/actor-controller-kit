using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(-1, 1)] public float Gravity = 1;

        protected Transform mainTransform;

        public float GetSpeedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getAcceleration => Acceleration > 0 ? Acceleration + 1 : 1;

        protected void Awake() => mainTransform = transform;

        public abstract void FreezAll();

        public abstract void FreezRotation();

        public abstract void MoveToDirection(Vector3 direction, float speed);

        public void MoveToPosition(Vector3 direction, float speed)
        {
            direction.Normalize();

            mainTransform.position += direction * speed * Time.fixedDeltaTime;
        }
    }
}