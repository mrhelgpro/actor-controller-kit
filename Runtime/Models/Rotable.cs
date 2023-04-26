using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum RotationMode { None, DirectionOfMovement, DirectionOfLook, Flip2D }

    [Serializable]
    public class Rotable : Model
    {
        public RotationMode Mode = RotationMode.None;

        public void Update(Vector3 moveDirection, Vector2 inputLook, float rate)
        {
            switch (Mode)
            {
                case RotationMode.None:
                    RootTransform.eulerAngles = Vector3.zero;
                    break;
                case RotationMode.DirectionOfMovement:
                    lookAtDirection(moveDirection, rate);
                    break;
                case RotationMode.DirectionOfLook:
                    directionByLook(moveDirection, inputLook, rate);
                    break;
                case RotationMode.Flip2D:
                    checkFlip(moveDirection);
                    break;
            }
        }

        private void directionByLook(Vector3 moveDirection, Vector2 inputLook, float rate)
        {
            if (moveDirection.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.Euler(0, inputLook.x, 0);
                RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, Time.deltaTime * 2.5f * rate);
            }
        }

        private void lookAtDirection(Vector3 moveDirection, float rate)
        {
            if (moveDirection.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, moveDirection.y, moveDirection.z), Vector3.up);
                RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, Time.deltaTime * 2.5f * rate);
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