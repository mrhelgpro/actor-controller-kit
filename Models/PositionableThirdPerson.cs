using UnityEngine;

namespace AssemblyActorCore
{
    public class PositionableThirdPerson : Positionable
    {
        protected override void GroundCheck()
        {
            IsGrounded = Physics.CheckSphere(mainTransform.position, radiusGroundCheck, groundLayer);
            SurfaceType = _getSurfaceType();
        }

        private string _getSurfaceType()
        {
            RaycastHit hit;

            float height = 0.1f;
            Vector3 origin = new Vector3(mainTransform.position.x, mainTransform.position.y + height, mainTransform.position.z);

            if (Physics.Raycast(origin, -Vector3.up, out hit, radiusGroundCheck + height, groundLayer))
            {
                return hit.collider.tag;
            }

            return "None";
        }

        private void OnCollisionEnter(Collision collision) => surfaceNormal = collision.contacts[0].normal;       
    }
}