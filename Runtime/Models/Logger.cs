using UnityEngine;

namespace AssemblyActorCore
{
    public class Logger : ActorComponent
    {
        public float Height;
        public float Speed;
        public bool RayCameraDirection = false;
        public bool RayMoveDirection = false;
        [Range (0, 100)] public int RayTraceTime = 0;

        protected Positionable positionable;

        private Vector3 _lastPositionForSpeed = Vector3.zero;
        private Transform _cameraTransform;

        private void Start()
        {
            positionable = GetComponentInParent<Positionable>();

            _cameraTransform = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            heightLog();
            speedLog();
            directionCameraLog();
            directionMoveLog();
            directionTraceLog();

            _lastPositionForSpeed = RootTransform.position;
        }

        private void heightLog()
        {
            if (positionable)
            {
                if (positionable.IsGrounded) Height = 0;

                if (RootTransform.position.y > Height)
                {
                    Height = Mathf.Round(RootTransform.position.y * 100f) / 100f;
                }
            }
            else
            {
                Debug.LogWarning(gameObject.name + " - Logger: <Positionable> is not found");
            }
        }


        private void speedLog()
        {
            Vector3 velocity = (RootTransform.position - _lastPositionForSpeed) / Time.deltaTime;
            Speed = Mathf.Round(velocity.magnitude * 100f) / 100f;
        }

        private void directionCameraLog()
        {
            if (RayCameraDirection)
            {
                float length = 1000;
                Vector3 origin = _cameraTransform.position;
                Vector3 direction = _cameraTransform.forward.normalized;
                
                RaycastHit hit;
                Physics.Raycast(origin, direction, out hit, length);

                if (hit.collider == null)
                {
                    Debug.DrawLine(origin, origin + direction * length, Color.white, 0, true);
                }
                else
                {
                    float distance = Vector3.Distance(origin, hit.point);
                    Debug.DrawLine(origin, origin + direction * distance, Color.white, 0, true);
                    Debug.DrawLine(hit.point, hit.point + direction * 1, Color.red, 0, true);
                }

                Debug.DrawLine(RootTransform.position, RootTransform.position + Vector3.ProjectOnPlane(direction, Vector3.up) * 10, Color.white, 0, true);
            }
        }

        private void directionMoveLog()
        {
            if (RayMoveDirection)
            {
                Debug.DrawLine(RootTransform.position, RootTransform.position + RootTransform.TransformDirection(Vector3.forward).normalized * 2, Color.blue, 0, true);
            }
        }

        private void directionTraceLog()
        {
            if (RayTraceTime > 0)
            {
                Vector3 direction = RootTransform.position - _lastPositionForSpeed;
                float distance = Vector3.Distance(RootTransform.position, _lastPositionForSpeed);

                Debug.DrawLine(RootTransform.position, RootTransform.position + direction.normalized * distance, Color.red, RayTraceTime, true);
            }
        }
    }
}