using UnityEngine;

namespace AssemblyActorCore
{
    public class Logger : MonoBehaviour
    {
        public bool LogHeight = false;
        public bool LogSpeed = false;
        public bool LogCameraDirection = false;

        protected Transform mainTransform;
        protected Positionable positionable;
        protected Directable directable;

        private void Awake()
        {
            mainTransform = transform;

            positionable = GetComponentInParent<Positionable>();
            directable = GetComponentInParent<Directable>();
        }

        private void FixedUpdate()
        {
            heightLog();
            speedLog();
            directionCameraLog();
        }

        private void directionCameraLog()
        {
            if (LogCameraDirection)
            {
                float length = 1000;
                Vector3 origin = Camera.main.transform.position;
                Vector3 direction = Camera.main.transform.forward.normalized;
                
                RaycastHit hit;
                Physics.Raycast(origin, direction, out hit, length);

                if (hit.collider == null)
                {
                    Debug.DrawLine(origin, origin + direction * length, Color.white, 0, false);
                }
                else
                {
                    float distance = Vector3.Distance(origin, hit.point);
                    Debug.DrawLine(origin, origin + direction * distance, Color.white, 0, false);
                    Debug.DrawLine(hit.point, hit.point + direction * 1, Color.red, 0, false);
                }
            }
        }


        private float _heightOfTheJump = 0;
        private void heightLog()
        {
            if (LogHeight)
            {
                if (positionable.IsGrounded) _heightOfTheJump = 0;

                if (mainTransform.position.y > _heightOfTheJump)
                {
                    _heightOfTheJump = mainTransform.position.y;
                    Debug.Log("Height of the jump = " + _heightOfTheJump + " (" + gameObject.name + ")");
                }
            }
        }

        private Vector3 _lastPositionForSpeed = Vector3.zero;
        private void speedLog()
        {
            if (LogSpeed)
            {
                Vector3 velocity = (mainTransform.position - _lastPositionForSpeed) / Time.deltaTime;
                float speed = velocity.magnitude;

                _lastPositionForSpeed = mainTransform.position;

                Debug.Log("Movement speed = " + speed + " (" + gameObject.name + ")");
            }
        }
    }
}