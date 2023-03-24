using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Positionable : MonoBehaviour
    {
        public bool IsGrounded;
        public string SurfaceType;
        [Range(0, 90)] public float CurrentSlope = 0;
        [Range(0, 89)] public float MaxSlope = 60;

        protected Vector3 surfaceNormal;
        protected const float radiusGroundCheck = 0.125f;
        protected const float lengthRaycast = 1.0f;
        protected LayerMask groundLayer;
        protected Transform mainTransform;

        protected void Awake()
        {
            groundLayer = LayerMask.GetMask("Default");
            mainTransform = transform;
        }

        private void FixedUpdate()
        {
            GroundCheck();

            CurrentSlope = Vector3.Angle(surfaceNormal, Vector3.up);
        }

        public Vector3 Project(Vector3 direction)
        {
            Vector3 projection = direction - Vector3.Dot(direction, surfaceNormal) * surfaceNormal;

            return projection == Vector3.zero || IsGrounded == false ? direction : projection;
        }

        protected abstract void GroundCheck();
    }
}