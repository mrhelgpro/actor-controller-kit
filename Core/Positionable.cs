using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Positionable : MonoBehaviour
    {
        public bool IsGrounded;
        public string SurfaceType;
        [Range(0, 90)] public float SurfaceSlope = 0;
        
        public bool IsNormalSlope => SurfaceSlope <= 46;

        protected Vector3 surfaceNormal;
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

            SurfaceSlope = Vector3.Angle(surfaceNormal, Vector3.up);
        }

        public Vector3 Project(Vector3 direction)
        {
            Vector3 projection = direction - Vector3.Dot(direction, surfaceNormal) * surfaceNormal;

            //Vector3 normal = projection == Vector3.zero || IsGrounded == false ? direction : projection;
            //Vector3 slope = Vector3.down - Vector3.Dot(Vector3.down, surfaceNormal) * surfaceNormal;
            //return IsNormalSlope ? normal : slope;

            return projection == Vector3.zero || IsGrounded == false ? direction : projection;
        }

        protected abstract void GroundCheck();
    }
}