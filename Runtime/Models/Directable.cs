using UnityEngine;

namespace AssemblyActorCore
{
    public class Directable : Model
    {
        public Vector3 GetCamera => _cameraDirection;
        public Vector3 GetBody => _bodyDirection;
        public Vector3 GetMove => _moveDirection;
        public Vector3 GetLocal => _localDirection;

        private Vector3 _cameraDirection;
        private Vector3 _bodyDirection;
        private Vector3 _moveDirection;
        private Vector3 _localDirection;

        private Transform _cameraTransform;
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

            _cameraTransform = UnityEngine.Camera.main.transform;
        }

        public void UpdateData(Vector2 inputMove, float rate)
        {
            _cameraDirection = _cameraTransform.forward.normalized;
            _bodyDirection = mainTransform.TransformDirection(Vector3.forward).normalized;
            _moveDirection = (Vector3.ProjectOnPlane(_cameraDirection, Vector3.up) * inputMove.y + _cameraTransform.right * inputMove.x).normalized; // Get direction relative to Camera
            _localDirection = _moveDirection.magnitude > 0 ? getShift(_moveDirection, _bodyDirection) : _localDirection;
        }
    }
}