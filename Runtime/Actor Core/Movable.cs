using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : ModelComponent
    {
        public MovementParametres MovementParametres;
        public Vector3 Velocity { get; protected set; }

        protected float maxSpeed = 0;    
        private float _speedScale = 1;
        private Vector3 _lerpDirection = Vector3.zero;

        private void FixedUpdate()
        {
            float speedScale = _speedScale < 0 ? 0 : _speedScale;
            maxSpeed = MovementParametres.Speed * speedScale;

            _lerpDirection = Vector3.Lerp(_lerpDirection, MovementParametres.Direction, Time.fixedDeltaTime * MovementParametres.Rate);
            Velocity = new Vector3(_lerpDirection.x, MovementParametres.Direction.y, _lerpDirection.z) * maxSpeed;

            Move();
        }

        public abstract void Exit();

        public void ChangeSpeed(float value) => _speedScale += value;
        public abstract void SetForce(Vector3 force);
        protected abstract void Move();
    }

    [Serializable]
    public class MovementParametres
    {
        public Vector3 Direction;
        public float Speed = 0f;
        [Range(1, 10)] public int Rate = 10;
        [Range(0, 2)] public float Gravity = 0;
    }
}