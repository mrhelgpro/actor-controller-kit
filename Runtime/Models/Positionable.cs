using UnityEngine;

namespace AssemblyActorCore
{
    public class Positionable : Model
    {
        public bool IsGrounded;
        public bool IsObstacle;
        public string SurfaceType = "None";
        public float SurfaceSlope;

        protected Vector3 surfaceNormal;
        protected LayerMask groundLayer;
        protected int layerMask;

        protected new void Awake()
        {
            base.Awake();

            groundLayer = LayerMask.GetMask("Default");
            layerMask = ~(1 << LayerMask.NameToLayer("Actor"));
        }

        public Vector3 ProjectOntoSurface(Vector3 direction)
        {
            Vector3 projection = direction - Vector3.Dot(direction, surfaceNormal) * surfaceNormal;
            return projection == Vector3.zero || IsGrounded == false ? direction : projection;
        }
    }
}