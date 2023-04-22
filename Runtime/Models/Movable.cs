using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : Model
    {
        public float GetCurrentSpeed => currentSpeed;

        protected Vector3 direction;
        protected float maxSpeed;
        protected float currentSpeed;
        protected float rate = 10;
        protected float gravity = 1;
        protected Vector3 velocity;
        
        private float _speedScale = 1;
        private Vector3 _previousPosition = Vector3.zero;
        private Vector3 _previousDirection = Vector3.zero;

        public void ChangeSpeed(float value) => _speedScale += value;

        public void UpdateParametres(Vector3 direction, float maxSpeed, float rate, float gravity, ref Vector3 force)
        {
            this.direction = direction;
            this.maxSpeed = maxSpeed;
            this.rate = rate;
            this.gravity = gravity;

            float speedScale = _speedScale < 0 ? 0 : _speedScale * Time.fixedDeltaTime;

            Vector3 smoothDirection = Vector3.Lerp(_previousDirection, direction, Time.fixedDeltaTime * rate);
            velocity = new Vector3(smoothDirection.x, direction.y, smoothDirection.z) * maxSpeed * speedScale;
            _previousDirection = smoothDirection;

            currentSpeed = ((mainTransform.position - _previousPosition) / Time.fixedDeltaTime).magnitude;
            _previousPosition = mainTransform.position;

            Move();

            if (force.magnitude > 0)
            {
                Force(ref force);
            } 
        }

        public abstract void Enable(bool state);
        protected abstract void Move();
        protected abstract void Force(ref Vector3 force);
    }
}