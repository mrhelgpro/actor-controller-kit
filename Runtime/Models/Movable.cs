using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : Model
    {
        //public float GetMoveSpeed;
        //public Vector3 GetVectorVelocity => _velocity;
        public float GetSpeedScale => _speedScale < 0 ? 0 : _speedScale * Time.fixedDeltaTime;

        public void ChangeSpeed(float value) => _speedScale += value;

        public virtual float GetVelocity()
        {
            float velocity = ((mainTransform.position - _lastPositionForSpeed) / Time.fixedDeltaTime).magnitude;
            _lastPositionForSpeed = mainTransform.position;

            return velocity;
        }

        public Vector3 GetVelocity(Vector3 direction, float rate)
        {
            Vector3 smoothDirection = Vector3.Lerp(_lastDirectionForAcceleration, direction, Time.fixedDeltaTime * rate);
            _lastDirectionForAcceleration = smoothDirection;
            return new Vector3(smoothDirection.x, direction.y, smoothDirection.z);
        }

        private float _moveSpeed;
        private Vector3 _velocity;
        private float _speedScale = 1;
        private Vector3 _lastPositionForSpeed = Vector3.zero;
        private Vector3 _lastDirectionForAcceleration = Vector3.zero;

        protected new void Awake()
        {
            base.Awake();

            _lastPositionForSpeed = mainTransform.position;
        }

        /*
        public virtual void Enable(bool state)
        {

        }


        public virtual void Update(Vector3 direction, float speed, float rate, float gravity)
        {
            _moveSpeed = speed * GetSpeedScale;
            _velocity = GetVelocity(direction, rate) * _moveSpeed;
        }

        public virtual void Force(Vector3 force)
        {

        }
        */

        public abstract void SetMoving(bool state);
        public abstract void Horizontal(Vector3 direction, float speed, float rate, float gravity);
        public abstract void Vertical(float speed);
    }
}