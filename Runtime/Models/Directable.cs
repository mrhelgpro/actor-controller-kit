using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum LookMode { LookToDirectionOfCamera, LookToDirectionOfPointer, LookToDirectionOfStick }

    [Serializable]
    public class Directable : Model
    {
        public LookMode LookMode = LookMode.LookToDirectionOfCamera;
        public Vector3 Look { get; private set; }
        public Vector3 Camera { get; private set; }
        public Vector3 Body { get; private set; }
        public Vector3 Local { get; private set; }

        private Transform _cameraTransform;
        private float _previousPositionY;
        private float _previousLookDeltaMagnitude;
        private Vector3 _previousLookDirection;

        public override void Initialization(Transform transform)
        {
            base.Initialization(transform);
            _cameraTransform = UnityEngine.Camera.main.transform;
        }

        public void Update(Vector2 inputMoveVector, Vector2 lookDelta, float rate)
        {
            setLookDirection(lookDelta);
            setLocalDirection(inputMoveVector, rate);

            Camera = _cameraTransform.forward.normalized;
            Body = RootTransform.TransformDirection(Vector3.forward).normalized;
        }

        private void setLookDirection(Vector2 lookDelta)
        {
            if (LookMode == LookMode.LookToDirectionOfCamera)
            {
                Look = Camera;
            }
            else if (LookMode == LookMode.LookToDirectionOfPointer)
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 lookDirection = UnityEngine.Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, UnityEngine.Camera.main.transform.position.y)) - RootTransform.position;
                Look = Vector3.ProjectOnPlane(lookDirection, Vector3.up).normalized;
            }
            else if (LookMode == LookMode.LookToDirectionOfStick)
            {
                if (lookDelta.magnitude > 0.1f)
                {
                    if (lookDelta.magnitude > _previousLookDeltaMagnitude)
                    {
                        _previousLookDirection = new Vector3(lookDelta.x, 0, -lookDelta.y).normalized;
                    } 
                }

                Look = Vector3.Lerp(Look, _previousLookDirection, Time.deltaTime * 15);
            }

            _previousLookDeltaMagnitude = lookDelta.magnitude;

            Debug.DrawLine(RootTransform.position, RootTransform.position + Look.normalized * 5, Color.green, 0, true);
        }

        private void setLocalDirection(Vector2 inputMoveVector, float rate)
        {
            float positionY = RootTransform.position.y;
            bool difference = Mathf.Abs(positionY - _previousPositionY) > 0.01f;

            float x = Mathf.Round(Vector3.Cross(inputMoveVector, Body.GetVector2Horizontal()).z);
            float z = Mathf.Round(Vector3.Dot(inputMoveVector, Body));
            float y = positionY > _previousPositionY ? 1 : difference ? -1 : 0;

            _previousPositionY = positionY;

            Local = inputMoveVector.magnitude > 0 ? new Vector3(x, y, z) : Local;
        }
    }
}