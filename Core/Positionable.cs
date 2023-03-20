using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Positionable : MonoBehaviour
    {
        public bool IsGrounded;
        public string SurfaceType;
        [Range(0, 90)] public float SurfaceAngle;
        public bool StayOnTheGround;

        protected const float radiusGroundCheck = 0.025f;
        protected LayerMask groundLayer;
        protected Transform myTransform;

        private Vector3 _lastGroundPosition;

        private void Awake()
        {
            groundLayer = LayerMask.GetMask("Default");
            myTransform = transform;
        }

        private void Update()
        {
            GroundCheck();

            stayOnTheGroung();
        }

        private void stayOnTheGroung()
        {
            if (StayOnTheGround)
            {
                if (IsGrounded == false)
                {
                    myTransform.position = _lastGroundPosition;
                }

                _lastGroundPosition = IsGrounded ? myTransform.position : _lastGroundPosition;
            }
        }

        protected abstract void GroundCheck();
    }
}