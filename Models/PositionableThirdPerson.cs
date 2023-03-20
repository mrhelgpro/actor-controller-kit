using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionableThirdPerson : Positionable
    {
        private void Update()
        {
            IsGrounded = Physics.CheckSphere(myTransform.position, 0.1f, groundLayer);

            SurfaceAngle = _getSurfaceAngle();
        }

        private float _getSurfaceAngle()
        {
            RaycastHit hit;

            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, 1f))
            {
                return Vector3.Angle(hit.normal, Vector3.up);
            }

            return 0f;
        }
    }
}