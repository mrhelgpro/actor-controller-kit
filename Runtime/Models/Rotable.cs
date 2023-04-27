using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum RotationMode { None, DirectionOfMovement, DirectionOfLook, Flip2D }

    [Serializable]
    public class Rotable : Model
    {
        public RotationMode Mode = RotationMode.None;

        public void Update(Vector3 moveDirection, Vector3 lookDirection, float rate)
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
                    directionByLook(moveDirection, lookDirection, rate);
                    break;
                case RotationMode.Flip2D:
                    checkFlip(moveDirection);
                    break;
            }
        }

        private void directionByLook(Vector3 moveDirection, Vector3 lookDirection, float rate)
        {
            if (moveDirection.magnitude > 0)
            {
                //Quaternion targetRotation = Quaternion.Euler(0, lookDirection.x, 0);
                //RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, Time.deltaTime * 2.5f * rate);

                Vector3 look = Vector3.Normalize(Vector3.Scale(Vector3.ProjectOnPlane(lookDirection, Vector3.up), new Vector3(1, 0, 1)));
                RootTransform.rotation = Quaternion.LookRotation(look, Vector3.up);
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