using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum RotateMode { None, RotateToMovement, RotateToLook, Flip2D }

    [Serializable]
    public class Rotable : Model
    {
        public RotateMode RotateMode = RotateMode.None;

        public void Update(Vector2 inputMoveVector, Vector3 lookDirection, float rate)
        {
            switch (RotateMode)
            {
                case RotateMode.None:
                    RootTransform.eulerAngles = Vector3.zero;
                    break;
                case RotateMode.RotateToMovement:
                    rotateToMovement(inputMoveVector, rate);
                    break;
                case RotateMode.RotateToLook:
                    rotateToLook(inputMoveVector, lookDirection, rate);
                    break;
                case RotateMode.Flip2D:
                    checkFlip(inputMoveVector);
                    break;
            }
        }

        private void rotateToLook(Vector2 inputMoveVector, Vector3 lookDirection, float rate)
        {
            if (inputMoveVector.magnitude > 0)
            {
                Vector3 look = Vector3.Normalize(Vector3.Scale(Vector3.ProjectOnPlane(lookDirection, Vector3.up), new Vector3(1, 0, 1)));
                RootTransform.rotation = Quaternion.LookRotation(look, Vector3.up);
            }
        }

        private void rotateToMovement(Vector2 inputMoveVector, float rate)
        {
            if (inputMoveVector.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(inputMoveVector.x, 0, inputMoveVector.y), Vector3.up);
                RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, Time.deltaTime * rate);
            }
        }

        private void checkFlip(Vector2 moveDirection)
        {
            if (RootTransform.localScale.z < 0 && moveDirection.x > 0)
            {
                flip();
            }
            else if (RootTransform.localScale.z > 0 && moveDirection.x < 0)
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