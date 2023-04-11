using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : Model
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 2)] public float Boost = 0;
        [Range(1, 10)] public float Acceleration = 10;
        [Range(0, 2)] public float Gravity = 1;

        [HideInInspector] public bool IsFall = false;
        [HideInInspector] public bool IsJump = false;

        public virtual float GetVelocity
        {
            get
            {
                float velocity = ((mainTransform.position - _lastPositionForSpeed) / Time.fixedDeltaTime).magnitude;
                _lastPositionForSpeed = mainTransform.position;

                return velocity;
            }
        }
        public virtual Vector3 GetDirection(Vector3 direction)
        {
            Vector3 currentDirection = Vector3.Lerp(_lastDirectionForAcceleration, direction, Time.fixedDeltaTime * Acceleration * 2);

            _lastDirectionForAcceleration = currentDirection;

            return currentDirection;
        }

        protected float getSpeedSliding(float slope = 45.0f) => slope * 0.1f * Time.fixedDeltaTime;
        protected float getSpeedScale => _getBoost * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getBoost => Boost > 0 ? Boost + 1 : 1;

        private Vector3 _lastPositionForSpeed = Vector3.zero;
        private Vector3 _lastDirectionForAcceleration = Vector3.zero;

        protected new void Awake()
        {
            base.Awake();

            _lastPositionForSpeed = mainTransform.position;
        }

        public abstract void StartMovement();
        public abstract void StopMovement();
        public abstract void MoveToDirection(Vector3 direction, float speed);
        public abstract void Jump(float force);
        public void MoveToPosition(Vector3 direction, float speed) => mainTransform.position += direction.normalized * speed * Time.fixedDeltaTime;
    }
}