using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum LookMode { Camera, Pointer, Stick }

    [Serializable]
    public class Directable : Model
    {
        public LookMode LookMode = LookMode.Camera;
        public Vector3 Look { get; private set; }
        public Vector3 Camera { get; private set; }
        public Vector3 Body { get; private set; }
        public Vector3 Move { get; private set; }
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

        public void Update(Vector2 inputMove, Vector2 lookDelta, float rate, bool isTarget)
        {
            setLookDirection(lookDelta);
            setMoveDirection(inputMove, isTarget);

            Camera = _cameraTransform.forward.normalized;
            Body = RootTransform.TransformDirection(Vector3.forward).normalized;
            
            Local = Move.magnitude > 0 ? getLocalDirection(Move, Body) : Local;
        }

        private void setMoveDirection(Vector2 inputMove, bool isTarget)
        {
            if (isTarget)
            {
                Move = new Vector3(inputMove.x, 0, inputMove.y);
            }
            else 
            {
                Move = (Vector3.ProjectOnPlane(Camera, Vector3.up) * inputMove.y + _cameraTransform.right * inputMove.x).normalized;
            }
        }

        private void setLookDirection(Vector2 lookDelta)
        {
            if (LookMode == LookMode.Camera)
            {
                Look = Camera;
            }
            else if (LookMode == LookMode.Pointer)
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 lookDirection = UnityEngine.Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, UnityEngine.Camera.main.transform.position.y)) - RootTransform.position;
                Look = Vector3.ProjectOnPlane(lookDirection, Vector3.up).normalized;
            }
            else if (LookMode == LookMode.Stick)
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