using UnityEngine.AI;
using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class MovableNavigation : Movable
    {
        public override float GetVelocity() => _navMeshAgent.velocity.magnitude;

        private NavMeshAgent _navMeshAgent;

        private new void Awake()
        {
            base.Awake();

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
        }

        public override void SetMoving(bool state) { }


        public override void Horizontal(Vector3 direction, float speed, float rate, float gravity)
        {
            _navMeshAgent.speed = speed;
            _navMeshAgent.acceleration = rate * 2;
            _navMeshAgent.SetDestination(mainTransform.position + direction.normalized);
        }

        public override void Vertical(float force) { }
    }
}