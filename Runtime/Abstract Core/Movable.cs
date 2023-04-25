using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : Model
    {
        public Vector3 Velocity { get; protected set; }

        protected Vector3 direction;
        protected float maxSpeed = 0;    
        protected float rate = 10;
        protected float gravity = 1;

        private float _speedScale = 1;
        private Vector3 _lerpDirection = Vector3.zero;

        public void SetParametersy(Vector3 direction, float speed, float gravity, float rate)
        {
            this.direction = direction;

            float speedScale = _speedScale < 0 ? 0 : _speedScale;
            maxSpeed = speed * speedScale;

            this.gravity = gravity;
            this.rate = rate;

            _lerpDirection = Vector3.Lerp(_lerpDirection, direction, Time.fixedDeltaTime * rate);
            Velocity = new Vector3(_lerpDirection.x, direction.y, _lerpDirection.z) * maxSpeed;

            Move();
        }

        public void ChangeSpeed(float value) => _speedScale += value;

        public abstract void Enable(bool state);

        public abstract void SetForce(Vector3 force);
        protected abstract void Move();
    }
}