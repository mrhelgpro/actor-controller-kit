using UnityEngine;

namespace AssemblyActorCore
{
    public class Logger : MonoBehaviour
    {
        public bool LogHeight = false;
        public bool LogSpeed = false;
        //public bool LogDirection = false;

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
            directionLog();
        }

        private void directionLog()
        {
            //Debug.DrawLine(mainTransform.position, mainTransform.position + positionable.surfaceNormal * 5, Color.white, 0, false);
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