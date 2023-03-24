using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(0, 2)] public float Gravity = 1;

        public bool IsFall = false;
        public bool IsJump = false;

        public float GetVelocity
        {
            get
            {
                float velocity = ((mainTransform.position - _lastPositionForSpeed) / Time.deltaTime).magnitude;
                _lastPositionForSpeed = mainTransform.position;

                return velocity;
            }
        }

        protected Transform mainTransform;

        protected float getSpeedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getAcceleration => Acceleration > 0 ? Acceleration + 1 : 1;

        private Vector3 _lastPositionForSpeed = Vector3.zero;

        protected void Awake()
        {
            mainTransform = transform;
            _lastPositionForSpeed = mainTransform.position;
        }

        public abstract void FreezAll();
        public abstract void FreezRotation();
        public abstract void MoveToDirection(Vector3 direction, float speed, bool isGrounded = true);
        public abstract void Jump(float force);
        public void MoveToPosition(Vector3 direction, float speed) => mainTransform.position += direction.normalized * speed * Time.fixedDeltaTime;
    }
}