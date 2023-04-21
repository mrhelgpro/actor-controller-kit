using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum RotationMode { None, DirectionOfMovement, DirectionOfLook, Flip2D }

    public class Rotable : Model
    {
        public void UpdateData(RotationMode mode, Vector3 moveDirection, Vector2 inputLook, float rate)
        {
            switch (mode)
            {
                case RotationMode.None:
                    mainTransform.transform.eulerAngles = Vector3.zero;
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
                mainTransform.rotation = Quaternion.Slerp(mainTransform.rotation, targetRotation, Time.deltaTime * 2.5f * rate);
            }
        }

        private void lookAtDirection(Vector3 moveDirection, float rate)
        {
            if (moveDirection.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, moveDirection.y, moveDirection.z), Vector3.up);
                mainTransform.rotation = Quaternion.Slerp(mainTransform.rotation, targetRotation, Time.deltaTime * 2.5f * rate);
            }
        }

        private void checkFlip(Vector2 moveDirection)
        {
            if (mainTransform.localScale.z < 0 && moveDirection.x > 0)
            {
                flip();
            }
            else if (mainTransform.localScale.z > 0 && moveDirection.x < 0)
            {
                flip();
            }
        }

        private void flip()
        {
            mainTransform.transform.eulerAngles = new Vector3(0, 90, 0);
            Vector3 Scaler = mainTransform.localScale;
            Scaler.z *= -1;
            mainTransform.localScale = Scaler;
        }
    }
}