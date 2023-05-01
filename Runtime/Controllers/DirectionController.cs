using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum LookMode { LookToCamera, LookToPointer, LookToStick }
    public enum RotateMode { None, RotateToMovement, RotateToLook, Flip2D }

    public sealed class DirectionController : Presenter
    {
        public LookMode LookMode = LookMode.LookToCamera;
        public RotateMode RotateMode = RotateMode.RotateToMovement;
        [Range (0, 10)] public float Rate = 10;

        public Vector3 Look { get; private set; }
        public Vector3 Camera { get; private set; }
        public Vector3 Body { get; private set; }
        public Vector3 Local { get; private set; }

        // Model Components
        private Inputable _inputable;

        private Transform _cameraTransform;
        private float _previousPositionY;
        private float _previousLookDeltaMagnitude;
        private Vector3 _previousLookDirection;

        private new void Awake()
        {
            base.Awake();

            _cameraTransform = UnityEngine.Camera.main.transform;

            _inputable = RequireComponent<Inputable>();
        }

        public override void UpdateLoop()
        {
            //animatorable.SetFloat("DirectionX", directable.Local.x); //animatorable.SetFloat("DirectionX", directable.GetLocal.x, 0.1f);
            //animatorable.SetFloat("DirectionZ", directable.Local.z); //animatorable.SetFloat("DirectionZ", directable.GetLocal.z, 0.1f);

            Camera = _cameraTransform.forward.normalized;
            Body = RootTransform.TransformDirection(Vector3.forward).normalized;

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
                case RotateMode.Flip2D:
                    checkFlip();
                    break;
            }
        }

        // Direction
        private void setLookDirection()
        {
            if (LookMode == LookMode.LookToCamera)
            {
                Look = Camera;
            }
            else if (LookMode == LookMode.LookToPointer)
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 lookDirection = UnityEngine.Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, UnityEngine.Camera.main.transform.position.y)) - RootTransform.position;
                Look = Vector3.ProjectOnPlane(lookDirection, Vector3.up).normalized;
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

                Look = Vector3.Lerp(Look, _previousLookDirection, Time.deltaTime * 15);
            }

            _previousLookDeltaMagnitude = _inputable.LookDelta.magnitude;

            Debug.DrawLine(RootTransform.position, RootTransform.position + Look.normalized * 5, Color.green, 0, true);
        }

        private void setLocalDirection()
        {
            float positionY = RootTransform.position.y;
            bool difference = Mathf.Abs(positionY - _previousPositionY) > 0.01f;

            float x = Mathf.Round(Vector3.Cross(_inputable.MoveVector, Body.GetVector2Horizontal()).z);
            float z = Mathf.Round(Vector3.Dot(_inputable.MoveVector, Body));
            float y = positionY > _previousPositionY ? 1 : difference ? -1 : 0;

            _previousPositionY = positionY;

            Local = _inputable.MoveVector.magnitude > 0 ? new Vector3(x, y, z) : Local;
        }

        // Rotation
        private void rotateToLook()
        {
            if (_inputable.MoveVector.magnitude > 0)
            {
                Vector3 direction = Vector3.Normalize(Vector3.Scale(Vector3.ProjectOnPlane(Look, Vector3.up), new Vector3(1, 0, 1)));
                
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, Time.deltaTime * Rate);
            }
        }

        private void rotateToMovement()
        {
            if (_inputable.MoveVector.magnitude > 0)
            {
                Vector3 direction = new Vector3(_inputable.MoveVector.x, 0, _inputable.MoveVector.y);

                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, Time.deltaTime * Rate);
            }
        }

        private void checkFlip()
        {
            if (RootTransform.localScale.z < 0 && _inputable.MoveVector.x > 0)
            {
                flip();
            }
            else if (RootTransform.localScale.z > 0 && _inputable.MoveVector.x < 0)
            {
                flip();
            }
        }

        private void flip()
        {
            RootTransform.transform.eulerAngles = new Vector3(0, 90, 0);
            Vector3 Scaler = RootTransform.localScale;
            Scaler.z *= -1;
            RootTransform.localScale = Scaler;
        }
    }
}