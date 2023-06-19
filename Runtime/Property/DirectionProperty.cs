using UnityEngine;

namespace Actormachine
{
    public enum LookMode { LookToCamera, LookToPointer, LookToStick }
    public enum RotateMode { None, RotateToMovement, RotateToLook }

    [AddComponentMenu("Actormachine/Property/Direction Property")]
    public sealed class DirectionProperty : Property
    {
        public LookMode LookMode = LookMode.LookToCamera;
        public RotateMode RotateMode = RotateMode.RotateToMovement;
        [Range (1, 10)] public int Rate = 10;

        // Direction Fields
        private float _rotationAngle;
        private Vector3 _rotationDirection;
        private Vector3 _lookDirection;
        private Vector3 _cameraDirection;
        private Vector3 _bodyDirection;

        // Buffer Fields
        private Transform _cameraTransform;
        private float _previousRotationAngle;
        private float _previousPositionY;
        private float _previousLookDeltaMagnitude;
        private Vector3 _previousLookDirection;
        private Vector3 _previousLocalDirection;

        private int _layerMask;

        // Model Components
        private Inputable _inputable;
        private Animatorable _animatorable;

        public override void OnEnableState()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Default");

            // Add or Get comppnent in the Root
            _cameraTransform = Camera.main.transform;
            _inputable = AddComponentInRoot<Inputable>();
            _animatorable = AddComponentInRoot<Animatorable>();
        }

        public override void OnEnterState()
        {
            _rotationDirection = RootTransform.TransformDirection(Vector3.forward).normalized;
            _previousRotationAngle = RootTransform.rotation.eulerAngles.y;
        }

        public override void OnActiveState()
        {
            _cameraDirection = _cameraTransform.forward.normalized;
            _bodyDirection = RootTransform.TransformDirection(Vector3.forward).normalized;

            setRotaryDirection();
            setLookDirection();
            setLocalDirection();

            switch (RotateMode)
            {
                case RotateMode.None:
                    RootTransform.eulerAngles = Vector3.zero;
                    break;

                case RotateMode.RotateToMovement:
                    rotateToMovement();
                    break;

                case RotateMode.RotateToLook:
                    rotateToLook();
                    break;
            }
        }

        // Direction
        private void setRotaryDirection()
        {
            float currentAngle = Mathf.Clamp(RootTransform.rotation.eulerAngles.y - _previousRotationAngle, -1, +1);
            _rotationAngle = Mathf.Lerp(_rotationAngle, currentAngle, Time.deltaTime * Rate * 2);
            _previousRotationAngle = RootTransform.rotation.eulerAngles.y;

            _animatorable.Rotation = _rotationAngle;
        }
        private void setLookDirection()
        {
            if (LookMode == LookMode.LookToCamera)
            {
                _lookDirection = _cameraDirection;
            }
            else if (LookMode == LookMode.LookToPointer)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(PointerScreen.GetPosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    Vector3 lookDirection = hit.point - RootTransform.position;
                    _lookDirection = Vector3.ProjectOnPlane(lookDirection, Vector3.up).normalized;

                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                }
            }
            else if (LookMode == LookMode.LookToStick)
            {
                if (_inputable.LookDelta.magnitude > 0.1f)
                {
                    if (_inputable.LookDelta.magnitude > _previousLookDeltaMagnitude)
                    {
                        _previousLookDirection = new Vector3(_inputable.LookDelta.x, 0, -_inputable.LookDelta.y).normalized;
                    }
                }

                _lookDirection = Vector3.Lerp(_lookDirection, _previousLookDirection, Time.deltaTime * 15); // !!!!!!!!
            }

            _previousLookDeltaMagnitude = _inputable.LookDelta.magnitude;

            Debug.DrawLine(RootTransform.position, RootTransform.position + _lookDirection.normalized * 5, Color.green, 0, true);
        }

        private void setLocalDirection()
        {
            // Calculate the current direction
            float positionY = RootTransform.position.y;
            bool difference = Mathf.Abs(positionY - _previousPositionY) > 0.01f;

            float x = Vector3.Cross(_inputable.MoveVector, new Vector2(_bodyDirection.x, _bodyDirection.z)).z;
            float z = Vector3.Dot(new Vector3(_inputable.MoveVector.x, 0, _inputable.MoveVector.y), _bodyDirection);
            float y = positionY > _previousPositionY ? 1 : difference ? -1 : 0;

            // Calculate the lerp direction
            Vector3 lerpLocalDirection = Vector3.Lerp(_previousLocalDirection, new Vector3(x, y, z), Time.deltaTime * Rate);

            // Get local direction
            _animatorable.Direction = _inputable.MoveVector.magnitude > 0 ? lerpLocalDirection : _animatorable.Direction;

            // Save the previous value
            _previousLocalDirection = _animatorable.Direction;
            _previousPositionY = positionY;
        }

        // Rotation
        private void rotateToLook()
        {
            _rotationDirection = Vector3.Normalize(Vector3.Scale(Vector3.ProjectOnPlane(_lookDirection, Vector3.up), new Vector3(1, 0, 1)));

            Quaternion targetRotation = Quaternion.LookRotation(_rotationDirection, Vector3.up);
            RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, Time.deltaTime * Rate);
        }

        private void rotateToMovement()
        {
            if (_inputable.MoveVector.magnitude > 0)
            {
                _rotationDirection = new Vector3(_inputable.MoveVector.x, 0, _inputable.MoveVector.y);
            }

            Quaternion targetRotation = Quaternion.LookRotation(_rotationDirection, Vector3.up);
            RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, Time.deltaTime * Rate);
        }
    }
}