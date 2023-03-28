using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class Rotable : MonoBehaviour
    {
        public enum RotationMode { None, Move, Look, Flip }
        public RotationMode Mode = RotationMode.Move;
        
        private Inputable _inputable;
        private Transform _mainTransform;
        private Vector3 _currentDirection;

        private void Awake()
        {
            _mainTransform = transform;
            _inputable = GetComponent<Inputable>();
        }

        private void FixedUpdate()
        {
            switch (Mode)
            {
                case RotationMode.None:
                    _mainTransform.transform.eulerAngles = Vector3.zero;
                    break;
                case RotationMode.Move:
                    directionByMove();
                    lookAtDirection();
                    break;
                case RotationMode.Look:
                    directionByLook();
                    lookAtDirection();
                    break;
                case RotationMode.Flip:
                    directionByMove();
                    checkFlip();
                    break;
            }
        }

        private void directionByMove()
        {
            _currentDirection = new Vector3(_inputable.Input.Move.x, 0.0f, _inputable.Input.Move.y);
        }

        private void directionByLook()
        {

        }

        private void lookAtDirection()
        {
            if (_currentDirection.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_currentDirection.x, _currentDirection.y, _currentDirection.z), Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 25);
            }
        }

        private void checkFlip()
        {
            if (_mainTransform.localScale.z < 0 && _currentDirection.x > 0)
            {
                flip();
            }
            else if (_mainTransform.localScale.z > 0 && _currentDirection.x < 0)
            {
                flip();
            }
        }

        private void flip()
        {
            _mainTransform.transform.eulerAngles = new Vector3(0, 90, 0);
            Vector3 Scaler = _mainTransform.localScale;
            Scaler.z *= -1;
            _mainTransform.localScale = Scaler;
        }
    }
}