using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Positionable : Model
    {
        public bool IsGrounded { get; protected set; }
        public bool IsObstacle { get; protected set; }
        public string SurfaceType { get; protected set; } = "None";
        public Vector3 SurfaceNormal { get; protected set; }
        
        protected LayerMask groundLayer;
        protected int layerMask;

        protected new void Awake()
        {
            base.Awake();

            groundLayer = LayerMask.GetMask("Default");
            layerMask = ~(1 << LayerMask.NameToLayer("Actor"));
        }


        public float GetSlope => Vector3.Angle(SurfaceNormal, Vector3.up);

        public Vector3 ProjectOntoSurface(Vector3 direction)
        {
            Vector3 projection = Vector3.ProjectOnPlane(direction, SurfaceNormal);
            return projection == Vector3.zero || IsGrounded == false ? direction : projection;
        }

        public abstract void UpdateParametres();
    }
}