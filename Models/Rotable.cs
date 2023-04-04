using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class Rotable : MonoBehaviour
    {
        public enum RotationMode { None, Move, Flip }
        public RotationMode Mode = RotationMode.Move;
        [Range (0, 10)] public int Rate = 5;

        private Transform _mainTransform;
        private Vector3 _currentDirection;

        private void Awake()
        {
            _mainTransform = transform;
        }

        public void UpdateModel(Vector3 move, Vector3 look = new Vector3())
        {
            switch (Mode)
            {
                case RotationMode.None:
                    _mainTransform.transform.eulerAngles = Vector3.zero;
                    break;
                case RotationMode.Move:
                    directionByMove(move);
                    lookAtDirection();
                    break;
                case RotationMode.Flip:
                    directionByMove(move);
                    checkFlip();
                    break;
            }
        }

        private void directionByMove(Vector3 move) => _currentDirection = move;

        private void lookAtDirection()
        {
            if (_currentDirection.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_currentDirection.x, _currentDirection.y, _currentDirection.z), Vector3.up);
                _mainTransform.rotation = Quaternion.Slerp(_mainTransform.rotation, targetRotation, Time.deltaTime * 2.5f * Rate);
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