using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Positionable : Model
    {
        public bool IsGrounded;
        public bool IsObstacle;
        public bool IsSliding;
        public string SurfaceType = "None";
        public const float MaxSlope = 46;

        public float SurfaceSlope => Vector3.Angle(surfaceNormal, Vector3.up);

        protected Vector3 surfaceNormal;
        protected LayerMask groundLayer;
        protected int layerMask;

        private float _timerSliding = 0;

        protected new void Awake()
        {
            base.Awake();

            groundLayer = LayerMask.GetMask("Default");
            layerMask = ~(1 << LayerMask.NameToLayer("Actor"));
            mainTransform = transform;
        }

        public abstract void UpdateModel();

        protected void slidingCheck()
        {
            float startTime = 0.1f;
            float endTime = 0.2f;

            if (IsGrounded == false) _timerSliding = startTime;

            if (IsSliding)
            {
                _timerSliding = SurfaceSlope < MaxSlope ? _timerSliding - Time.fixedDeltaTime : endTime;
                IsSliding = _timerSliding <= 0.0f ? false : true;
                _timerSliding = IsSliding == false ? 0.0f : _timerSliding;
            }
            else
            {
                _timerSliding = SurfaceSlope > MaxSlope && SurfaceSlope < 75 ? _timerSliding + Time.fixedDeltaTime : 0.0f;
                IsSliding = _timerSliding > startTime ? true : false;
                _timerSliding = IsSliding ? endTime : _timerSliding;
            }
        }

        public Vector3 Project(Vector3 direction)
        {
            Vector3 projection = direction - Vector3.Dot(direction, surfaceNormal) * surfaceNormal;
            Vector3 normal = projection == Vector3.zero || IsGrounded == false || SurfaceSlope > 75 ? direction : projection;
            Vector3 slope = Vector3.down - Vector3.Dot(Vector3.down, surfaceNormal) * surfaceNormal;

            return IsSliding ? SurfaceSlope > 75 ? direction : slope : normal;
        }  
    }
}