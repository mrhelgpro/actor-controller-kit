using UnityEngine.AI;
using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovableNavigation : Movable
    {
        public override float GetVelocity => _navMeshAgent.velocity.magnitude;

        private NavMeshAgent _navMeshAgent;

        private new void Awake()
        {
            base.Awake();

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
        }

        public override void StartMovement()
        {

        }

        public override void StopMovement()
        {

        }

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            _navMeshAgent.speed = speed;
            _navMeshAgent.acceleration = Acceleration * 2;
            _navMeshAgent.SetDestination(mainTransform.position + direction.normalized);
        }

        public override void Jump(float force) { }
    }
}