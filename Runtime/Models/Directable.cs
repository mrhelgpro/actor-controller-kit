using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class Directable : Model
    {
        public Vector3 Camera;
        public Vector3 Body;
        public Vector3 Move;
        public Vector3 Shift;

        public enum Mode { None, DirectionOfMovement, DirectionOfLook, Flip2D }
        public Mode RotationMode = Mode.DirectionOfMovement;
        [Range (0, 10)] public int RotationRate = 5;

        private Transform _camera;
        private float _previousPositionY;
        private Vector3 getShift(Vector3 move, Vector3 body)
        {
            float positionY = mainTransform.position.y;
            bool difference = Mathf.Abs(positionY - _previousPositionY) > 0.01f;

            float x = Mathf.Round(Vector3.Cross(move.GetVector2Horizontal(), body.GetVector2Horizontal()).z);
            float z = Mathf.Round(Vector3.Dot(move, body));
            float y = positionY > _previousPositionY ? 1 : difference ? -1 : 0;

            _previousPositionY = positionY;

            return new Vector3(x, y, z);
        }

        private new void Awake()
        {
            base.Awake();

            _camera = UnityEngine.Camera.main.transform;
        }

        public void UpdateData(Vector2 inputMove, Vector2 inputLook)
        {
            Camera = _camera.forward.normalized;

            Body = mainTransform.TransformDirection(Vector3.forward).normalized;
            Move = (Vector3.ProjectOnPlane(Camera, Vector3.up) * inputMove.y + _camera.right * inputMove.x).normalized; // Get direction relative to Camera
            Shift = Move.magnitude > 0 ? getShift(Move, Body) : Shift;

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
                case Mode.Flip2D:
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