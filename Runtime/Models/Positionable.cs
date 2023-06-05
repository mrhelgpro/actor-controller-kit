using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - to get the position data. </summary>
    public class Positionable : ModelBehaviour
    {
        // Surface
        public bool IsGrounded = true;
        public Transform SurfaceTransform = null;
        public string SurfaceType = "None";
        public Vector3 SurfaceNormal = Vector3.zero;
        
        // Obstacle
        public bool IsObstacle = false;
        public Transform ObstacleTransform = null;

        // Edge
        public bool IsEdge = false;

        // Abyss
        public bool IsAbyss = false;

        protected LayerMask groundLayer;
        protected int layerMask;
        protected float radiusGroundCheck = 0.2f;

        private void Start()
        {
            groundLayer = LayerMask.GetMask("Default");
            layerMask = 1 << LayerMask.NameToLayer("Default");
        }

        private void Update()
        {
            GroundCheck();
        }

        private void FixedUpdate()
        {
            SurfaceCheck();
            ObstacleCheck();
            AbyssCheck();
        }

        public float GetSlope => Vector3.Angle(SurfaceNormal, Vector3.up);

        public Vector3 GetDirection(Vector2 inputMoveVector)
        {
            Vector3 direction = new Vector3(inputMoveVector.x, 0, inputMoveVector.y);
            Vector3 projection = Vector3.ProjectOnPlane(direction, SurfaceNormal);
            Vector3 result = projection == Vector3.zero || IsGrounded == false ? direction : projection;
            
            return result.normalized;
        }

        protected virtual void GroundCheck()
        {
            IsGrounded = Physics.CheckSphere(RootTransform.position, radiusGroundCheck * 0.75f, groundLayer);
        }

        protected virtual void SurfaceCheck()
        {
            float offsetHeight = radiusGroundCheck;
            float length = 2.0f;
            RaycastHit hit;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + offsetHeight, RootTransform.position.z);
            Physics.Raycast(origin, Vector3.down, out hit, length, layerMask);

            SurfaceTransform = IsGrounded ? hit.transform : null;
            SurfaceType = hit.collider != null ? hit.collider.tag : "None";
            SurfaceNormal = hit.collider != null ? hit.normal : Vector3.zero;
        }

        protected virtual void ObstacleCheck()
        {
            float offsetHeight = 0.25f;
            float length = 0.25f;
            RaycastHit hit;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + offsetHeight, RootTransform.position.z);
            Physics.Raycast(origin, RootTransform.TransformDirection(Vector3.forward), out hit, length, layerMask);

            IsObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
            ObstacleTransform = IsObstacle ? hit.transform : null;
        }

        protected virtual void AbyssCheck()
        {
            float offsetHeight = 0.5f;
            float length = 3.125f + offsetHeight;
            float offsetForward = 0.35f;
            float edgeDistance = 0.5f + offsetHeight;
            RaycastHit hit;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + offsetHeight, RootTransform.position.z) + (RootTransform.TransformDirection(Vector3.forward) * offsetForward);
            Physics.Raycast(origin, Vector3.down, out hit, length, layerMask);

            IsAbyss = IsGrounded == false ? false : hit.collider == null;
            IsEdge = IsGrounded == false ? false : hit.collider != null && hit.distance > edgeDistance;//hit.collider == null ? false : IsGrounded ? hit.distance > edgeDistance : false;
        }
    }
}