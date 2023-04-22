using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : Model
    {
        //public float GetMoveSpeed;
        //public Vector3 GetVectorVelocity => _velocity;

        public virtual float GetVelocity()
        {
            float velocity = ((mainTransform.position - _lastPositionForSpeed) / Time.fixedDeltaTime).magnitude;
            _lastPositionForSpeed = mainTransform.position;

            return velocity;
        }

        protected Vector3 direction;
        protected float speed;  
        protected float rate;
        protected float gravity;
        protected Vector3 velocity;
        protected float speedScale = 1;

        private Vector3 _lastPositionForSpeed = Vector3.zero;
        private Vector3 _lastDirectionForAcceleration = Vector3.zero;
        private float _getSpeedScale => speedScale < 0 ? 0 : speedScale * Time.fixedDeltaTime;
        private Vector3 _getVelocity(Vector3 direction, float rate)
        {
            Vector3 smoothDirection = Vector3.Lerp(_lastDirectionForAcceleration, direction, Time.fixedDeltaTime * rate);
            _lastDirectionForAcceleration = smoothDirection;
            return new Vector3(smoothDirection.x, direction.y, smoothDirection.z);
        }

        protected new void Awake()
        {
            base.Awake();

            _lastPositionForSpeed = mainTransform.position;
        }

        public void ChangeSpeed(float value) => speedScale += value;

        public void UpdateData(Vector3 direction, float speed, float rate, float gravity, ref float force)
        {
            this.direction = direction;
            this.speed = speed;
            this.rate = rate;
            this.gravity = gravity;

            velocity = _getVelocity(direction, rate) * speed * _getSpeedScale;

            Move();

            if (force > 0)
            {
                Force(ref force);
            } 
        }

        public abstract void Enable(bool state);
        protected abstract void Move();
        protected abstract void Force(ref float force);
    }
}