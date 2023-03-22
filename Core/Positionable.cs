using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Positionable : MonoBehaviour
    {
        public bool IsGrounded;
        //public bool IsEdge;
        public string SurfaceType;
        [Range(0, 90)] public float SurfaceAngle;

        protected Vector3 surfaceNormal;
        protected const float radiusGroundCheck = 0.125f;
        protected const float lengthRaycast = 1.0f;
        protected LayerMask groundLayer;
        protected Transform mainTransform;

        private void Awake()
        {
            groundLayer = LayerMask.GetMask("Default");
            mainTransform = transform;
        }

        private void FixedUpdate()
        {
            GroundCheck();

            SurfaceAngle = Vector3.Angle(surfaceNormal, Vector3.up);
        }

        public Vector3 Project(Vector3 direction)
        {
            Vector3 projection = direction - Vector3.Dot(direction, surfaceNormal) * surfaceNormal;

            return projection == Vector3.zero || IsGrounded == false ? direction : projection;
        }

        protected abstract void GroundCheck();

        /*
        public bool StayOnTheGround;
        private Vector3 _lastGroundPosition;

        private void stayOnTheGroung()
        {
            if (StayOnTheGround)
            {
                if (IsEdge == true)
                {
                    myTransform.position = _lastGroundPosition;

                    Debug.Log("STAY ON THE GROUND!!!!!!!!!!!!!!!!!!!!");
                }
                else
                {
                    _lastGroundPosition = myTransform.position;
                }

                //_lastGroundPosition = IsEdge == false ? myTransform.position : _lastGroundPosition;
            }
        }
        */
    }
}