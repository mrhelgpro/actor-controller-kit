using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Positionable : ModelComponent
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

        private void FixedUpdate() => UpdateParametres();

        public float GetSlope => Vector3.Angle(SurfaceNormal, Vector3.up);

        public Vector3 ProjectOntoSurface(Vector2 inputMoveVector)
        {
            Vector3 direction = new Vector3(inputMoveVector.x, 0, inputMoveVector.y);
            Vector3 projection = Vector3.ProjectOnPlane(direction, SurfaceNormal);
            return projection == Vector3.zero || IsGrounded == false ? direction : projection;
        }

        public abstract void UpdateParametres();
    }
}