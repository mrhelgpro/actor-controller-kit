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
        /*
        protected override void Move()
        {
            _navMeshAgent.speed = maxSpeed;
            _navMeshAgent.acceleration = MovementParametres.Rate * 2;
            _navMeshAgent.SetDestination(RootTransform.position + MovementParametres.Direction.normalized);
        }

        public override void Exit()
        {
            _navMeshAgent.speed = 0;
            _navMeshAgent.acceleration = 0;
            _navMeshAgent.SetDestination(RootTransform.position);
        }

        public override void SetForce(Vector3 force) { }
        */
    }
}