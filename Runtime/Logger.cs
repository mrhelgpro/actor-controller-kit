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

        private void Awake()
        {
            mainTransform = transform;

            positionable = GetComponentInParent<Positionable>();
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
                Vector3 cameraDirection = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
                Debug.DrawLine(transform.position, transform.position + cameraDirection.normalized * 3, Color.green, 0, false);
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