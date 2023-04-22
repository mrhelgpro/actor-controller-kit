using UnityEngine.AI;
using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovableNavigation : Movable
    {
        private NavMeshAgent _navMeshAgent;

        private new void Awake()
        {
            base.Awake();

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
        }

        public override void Enable(bool state) { }

        protected override void Move()
        {
            _navMeshAgent.speed = maxSpeed;
            _navMeshAgent.acceleration = rate * 2;
            _navMeshAgent.SetDestination(mainTransform.position + direction.normalized);
            currentSpeed = _navMeshAgent.velocity.magnitude;

        }

        protected override void Force(ref Vector3 force) { }
    }
}