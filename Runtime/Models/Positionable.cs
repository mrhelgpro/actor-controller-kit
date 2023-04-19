using UnityEngine;

namespace AssemblyActorCore
{
    public class Positionable : Model
    {
        public bool IsGrounded;
        public bool IsObstacle;
        public string SurfaceType = "None";
        public Vector3 SurfaceNormal;
        public float SurfaceSlope => Vector3.Angle(SurfaceNormal, Vector3.up);

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
            Vector3 projection = Vector3.ProjectOnPlane(direction, SurfaceNormal);
            return projection == Vector3.zero || IsGrounded == false ? direction : projection;
        }
    }
}