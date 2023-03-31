using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Positionable : MonoBehaviour
    {
        public bool IsGrounded;
        public bool IsObstacle;
        public bool IsSliding;
        public string SurfaceType = "None";
        [Range(1, 75)] public float MaxSlope = 45;

        public float SurfaceSlope => Vector3.Angle(surfaceNormal, Vector3.up);

        protected Vector3 surfaceNormal;
        protected LayerMask groundLayer;
        protected int layerMask;
        protected Transform mainTransform;
        protected Vector3 mainDirection => mainTransform.TransformDirection(Vector3.forward);

        public float _timerSliding = 0;

        protected void Awake()
        {
            groundLayer = LayerMask.GetMask("Default");
            layerMask = ~(1 << LayerMask.NameToLayer("Actor"));
            mainTransform = transform;
        }

        private void FixedUpdate()
        {
            UpdatePosition();

            slidingCheck();
        }

        private void slidingCheck()
        {
            if (IsSliding)
            {
                _timerSliding = SurfaceSlope < MaxSlope ? _timerSliding - Time.fixedDeltaTime : 0.2f;
                IsSliding = _timerSliding <= 0.0f ? false : true;
                _timerSliding = IsSliding == false ? 0.0f : _timerSliding;
            }
            else
            {
                _timerSliding = SurfaceSlope > MaxSlope && SurfaceSlope < 75 ? _timerSliding + Time.fixedDeltaTime : 0.0f;
                IsSliding = _timerSliding > 0.1f ? true : false;
                _timerSliding = IsSliding ? 0.2f : _timerSliding;
            }

            //_timerSliding = SurfaceSlope < MaxSlope ? Mathf.Max(_timerSliding - Time.fixedDeltaTime, 0.0f) : SurfaceSlope < 75 ? Mathf.Min(_timerSliding + Time.fixedDeltaTime, 0.2f) : 0.0f;
            //IsSliding = _timerSliding > 0.1f && SurfaceSlope > MaxSlope ? true : false;
        }

        public Vector3 Project(Vector3 direction)
        {
            Vector3 projection = direction - Vector3.Dot(direction, surfaceNormal) * surfaceNormal;
            Vector3 normal = projection == Vector3.zero || IsGrounded == false ? direction : projection;
            Vector3 slope = Vector3.down - Vector3.Dot(Vector3.down, surfaceNormal) * surfaceNormal;

            return IsSliding ? IsObstacle ? direction : slope : normal;

            //Vector3 projection = direction - Vector3.Dot(direction, surfaceNormal) * surfaceNormal;
            //return projection == Vector3.zero || IsGrounded == false || IsSliding ? direction : projection;
        }

        protected abstract void UpdatePosition();
    }
}