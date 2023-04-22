using UnityEngine;

namespace AssemblyActorCore
{
    public class Positionable : Model
    {
        public bool IsGrounded => isGrounded;
        public bool IsObstacle => isObstacle;
        public string SurfaceType => surfaceType;
        public Vector3 SurfaceNormal => surfaceNormal;
        public float GetSlope => Vector3.Angle(surfaceNormal, Vector3.up);

        protected bool isGrounded;
        protected bool isObstacle;
        protected string surfaceType = "None";
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
            Vector3 projection = Vector3.ProjectOnPlane(direction, surfaceNormal);
            return projection == Vector3.zero || isGrounded == false ? direction : projection;
        }

        public virtual void UpdateParametres() { }
    }
}