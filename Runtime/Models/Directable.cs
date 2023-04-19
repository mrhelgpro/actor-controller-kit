using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class Directable : Model
    {
        public Vector3 Camera;
        public Vector3 Body;
        public Vector3 Move;
        public Vector2 Strafe;

        public enum Mode { None, DirectionOfMovement, DirectionOfLook, Flip }
        public Mode RotationMode = Mode.DirectionOfMovement;
        [Range (0, 10)] public int RotationRate = 5;

        private Transform _camera;

        private new void Awake()
        {
            base.Awake();

            _camera = UnityEngine.Camera.main.transform;
        }

        public void UpdateData(Vector2 inputMove, Vector2 inputLook)
        {
            Camera = new Vector3(_camera.forward.x, 0f, _camera.forward.z).normalized;
            Body = mainTransform.TransformDirection(Vector3.forward).normalized;
            Move = (Camera * inputMove.y + _camera.right * inputMove.x).normalized; // Get direction relative to Camera
            Strafe = new Vector2(Mathf.Round(Vector3.Cross(Move.GetVector2Horizontal(), Body.GetVector2Horizontal()).z), Mathf.Round(Vector3.Dot(Move, Body)));

            Debug.DrawLine(transform.position, transform.position + Camera.normalized * 5, Color.white, 0, false);
            Debug.DrawLine(mainTransform.position, mainTransform.position + Move.normalized * 2, Color.yellow, 0, false);

            switch (RotationMode)
            {
                case Mode.None:
                    mainTransform.transform.eulerAngles = Vector3.zero;
                    break;
                case Mode.DirectionOfMovement:
                    lookAtDirection();
                    break;
                case Mode.DirectionOfLook:
                    directionByLook(inputLook);
                    break;
                case Mode.Flip:
                    checkFlip();
                    break;
            }
        }

        private void directionByLook(Vector2 inputLook)
        {
            if (Move.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.Euler(0, inputLook.x, 0);
                mainTransform.rotation = Quaternion.Slerp(mainTransform.rotation, targetRotation, Time.deltaTime * 2.5f * RotationRate);               
            }
        }

        private void lookAtDirection()
        {
            if (Move.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(Move.x, Move.y, Move.z), Vector3.up);
                mainTransform.rotation = Quaternion.Slerp(mainTransform.rotation, targetRotation, Time.deltaTime * 2.5f * RotationRate);
            }
        }

        private void checkFlip()
        {
            if (mainTransform.localScale.z < 0 && Move.x > 0)
            {
                flip();
            }
            else if (mainTransform.localScale.z > 0 && Move.x < 0)
            {
                flip();
            }
        }

        private void flip()
        {
            mainTransform.transform.eulerAngles = new Vector3(0, 90, 0);
            Vector3 Scaler = mainTransform.localScale;
            Scaler.z *= -1;
            mainTransform.localScale = Scaler;
        }
    }
}