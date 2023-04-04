using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class Rotable : MonoBehaviour
    {
        public enum RotationMode { None, Move, Look, Flip }
        public RotationMode Mode = RotationMode.Move;
        [Range (0, 10)] public int Rate = 5;

        private Transform _mainTransform;

        private void Awake()
        {
            _mainTransform = transform;
        }

        public void UpdateModel(Vector3 move, float look = 0)
        {
            switch (Mode)
            {
                case RotationMode.None:
                    _mainTransform.transform.eulerAngles = Vector3.zero;
                    break;
                case RotationMode.Move:
                    lookAtDirection(move);
                    break;
                case RotationMode.Look:
                    directionByLook(move, look);
                    break;
                case RotationMode.Flip:
                    checkFlip(move);
                    break;
            }
        }

        private void directionByLook(Vector3 move, float look)
        {
            if (move.z != 0)
            {
                Quaternion targetRotation = Quaternion.Euler(0, look, 0);
                _mainTransform.rotation = Quaternion.Slerp(_mainTransform.rotation, targetRotation, Time.deltaTime * 2.5f * Rate);
            }
        }

        private void lookAtDirection(Vector3 move)
        {
            if (move.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(move.x, move.y, move.z), Vector3.up);
                _mainTransform.rotation = Quaternion.Slerp(_mainTransform.rotation, targetRotation, Time.deltaTime * 2.5f * Rate);
            }
        }

        private void checkFlip(Vector3 move)
        {
            if (_mainTransform.localScale.z < 0 && move.x > 0)
            {
                flip();
            }
            else if (_mainTransform.localScale.z > 0 && move.x < 0)
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