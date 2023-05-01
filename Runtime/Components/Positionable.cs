using UnityEngine;

namespace AssemblyActorCore
{
    public class Positionable : ActorComponent
    {
        public bool IsGrounded { get; protected set; }
        public bool IsObstacle { get; protected set; }
        public string SurfaceType { get; protected set; } = "None";
        public Vector3 SurfaceNormal { get; protected set; }
        
        protected LayerMask groundLayer;
        protected int layerMask;

        private new void Awake()
        {
            base.Awake();

            groundLayer = LayerMask.GetMask("Default");
            layerMask = ~(1 << LayerMask.NameToLayer("Actor"));
        }

        private void Update()
        {
            GroundCheck();
            SurfaceCheck();
            ObstacleCheck();
        }

        public float GetSlope => Vector3.Angle(SurfaceNormal, Vector3.up);

        public Vector3 ProjectOntoSurface(Vector2 inputMoveVector)
        {
            Vector3 direction = new Vector3(inputMoveVector.x, 0, inputMoveVector.y);
            Vector3 projection = Vector3.ProjectOnPlane(direction, SurfaceNormal);
            return projection == Vector3.zero || IsGrounded == false ? direction : projection;
        }

        protected virtual void GroundCheck()
        {
            IsGrounded = Physics.CheckSphere(RootTransform.position, 0.2f, groundLayer);
        }

        protected virtual void SurfaceCheck()
        {
            float length = 2.0f;
            RaycastHit hit;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + 0.25f, RootTransform.position.z);
            Physics.Raycast(origin, Vector3.down, out hit, length);

            SurfaceType = hit.collider != null ? hit.collider.tag : "None";
            SurfaceNormal = hit.collider != null ? hit.normal : Vector3.zero;
        }

        protected virtual void ObstacleCheck()
        {
            float length = 0.35f;
            RaycastHit hit;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + 0.25f, RootTransform.position.z);
            Physics.Raycast(origin, RootTransform.TransformDirection(Vector3.forward), out hit, length);

            IsObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
        }
    }
}