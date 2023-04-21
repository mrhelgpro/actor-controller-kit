using UnityEngine;

namespace AssemblyActorCore
{
    public class Logger : Model
    {
        public float Height;
        public float Speed;
        public bool RayCameraDirection = false;
        public bool RayMoveDirection = false;
        [Range (0, 100)] public int RayTraceTime = 0;

        protected Positionable positionable;
        protected Directable directable;

        private Vector3 _lastPositionForSpeed = Vector3.zero;
        private Transform _cameraTransform;

        private new void Awake()
        {
            base.Awake();

            positionable = GetComponentInParent<Positionable>();
            directable = GetComponentInParent<Directable>();

            _cameraTransform = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            heightLog();
            speedLog();
            directionCameraLog();
            directionMoveLog();
            directionTraceLog();

            _lastPositionForSpeed = mainTransform.position;
        }

        private void heightLog()
        {
            if (positionable)
            {
                if (positionable.IsGrounded) Height = 0;

                if (mainTransform.position.y > Height)
                {
                    Height = Mathf.Round(mainTransform.position.y * 100f) / 100f;
                }
            }
            else
            {
                Debug.LogWarning(gameObject.name + " - Logger: <Positionable> is not found");
            }
        }


        private void speedLog()
        {
            Vector3 velocity = (mainTransform.position - _lastPositionForSpeed) / Time.deltaTime;
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

                Debug.DrawLine(mainTransform.position, mainTransform.position + Vector3.ProjectOnPlane(direction, Vector3.up) * 10, Color.white, 0, true);
            }
        }

        private void directionMoveLog()
        {
            if (RayMoveDirection)
            {
                if (directable)
                {
                    Debug.DrawLine(mainTransform.position, mainTransform.position + directable.GetMove * 2, Color.blue, 0, true);
                }
                else
                {
                    Debug.LogWarning(gameObject.name + " - Logger: <Directable> is not found");
                }
            }
        }

        private void directionTraceLog()
        {
            if (RayTraceTime > 0)
            {
                Vector3 direction = mainTransform.position - _lastPositionForSpeed;
                float distance = Vector3.Distance(mainTransform.position, _lastPositionForSpeed);

                Debug.DrawLine(mainTransform.position, mainTransform.position + direction.normalized * distance, Color.red, RayTraceTime, true);
            }
        }
    }
}