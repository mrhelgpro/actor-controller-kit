using UnityEngine.AI;
using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovableNavigation : Movable
    {
        //[Range(0, 5)] public int RotationSpeed = 3;

        private NavMeshAgent _navMeshAgent;

        private new void Awake()
        {
            base.Awake();

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
        }

        public override void FreezAll()
        {

        }

        public override void FreezRotation()
        {

        }

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            _navMeshAgent.speed = speed;
            _navMeshAgent.SetDestination(mainTransform.position + direction.normalized);

            //rotation(direction, speed);
        }

        /*
        private void rotation(Vector3 direction, float speed)
        {
            if (direction != Vector3.zero)
            {
                mainTransform.position += direction * 50 * speed * RotationSpeed * Time.fixedDeltaTime;

                Vector3 lookAtPosition = mainTransform.position + new Vector3(direction.x, 0f, direction.z);
                mainTransform.LookAt(lookAtPosition, Vector3.up);
            }
        }
        */

        public override void Jump(float force)
        {
            //IsJump = true;
        }
    }
}