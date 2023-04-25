using UnityEngine;

namespace AssemblyActorCore
{
    public class Directable : Model
    {
        public Vector3 Camera { get; private set; }
        public Vector3 Body { get; private set; }
        public Vector3 Move { get; private set; }
        public Vector3 Local { get; private set; }

        private Transform _cameraTransform;
        private float _previousPositionY;

        private new void Awake()
        {
            base.Awake();

            _cameraTransform = UnityEngine.Camera.main.transform;
        }

        public void SetParameters(Vector2 inputMove, float rate)
        {
            Camera = _cameraTransform.forward.normalized;
            Body = RootTransform.TransformDirection(Vector3.forward).normalized;
            Move = (Vector3.ProjectOnPlane(Camera, Vector3.up) * inputMove.y + _cameraTransform.right * inputMove.x).normalized; // Get direction relative to Camera
            Local = Move.magnitude > 0 ? getLocalDirection(Move, Body) : Local;
        }

        private Vector3 getLocalDirection(Vector3 move, Vector3 body)
        {
            float positionY = RootTransform.position.y;
            bool difference = Mathf.Abs(positionY - _previousPositionY) > 0.01f;

            float x = Mathf.Round(Vector3.Cross(move.GetVector2Horizontal(), body.GetVector2Horizontal()).z);
            float z = Mathf.Round(Vector3.Dot(move, body));
            float y = positionY > _previousPositionY ? 1 : difference ? -1 : 0;

            _previousPositionY = positionY;

            return new Vector3(x, y, z);
        }
    }
}