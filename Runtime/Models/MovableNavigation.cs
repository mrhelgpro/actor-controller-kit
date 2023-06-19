using UnityEngine.AI;
using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - for speed control. </summary>
    [AddComponentMenu("Actormachine/Model/MovableNavigation")]
    public class MovableNavigation : Movable
    {
        // Unity Components
        private NavMeshAgent _navMeshAgent;

        public override void Enable()
        {
            // Add or Get comppnent in the Root
            _navMeshAgent = AddComponentInRoot<NavMeshAgent>();
        }

        public override void Enter()
        {
            // Set Movement Parementers
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.agentTypeID = 0;
            _navMeshAgent.baseOffset = 0;
            _navMeshAgent.speed = 2f;
            _navMeshAgent.angularSpeed = 10000;
            _navMeshAgent.acceleration = 100;
            _navMeshAgent.stoppingDistance = 0;
            _navMeshAgent.autoBraking = true;
            _navMeshAgent.radius = 0.25f;
            _navMeshAgent.height = _navMeshAgent.radius * 2;
            _navMeshAgent.avoidancePriority = 50;
            _navMeshAgent.autoTraverseOffMeshLink = true;
            _navMeshAgent.autoRepath = true;
            _navMeshAgent.updateRotation = false;
        }

        public override void Horizontal(Vector3 direction, float speed, float rate)
        {
            _navMeshAgent.speed = speed;
            _navMeshAgent.acceleration = rate * 4;
            _navMeshAgent.SetDestination(RootTransform.position + direction.normalized);

            Velocity = _navMeshAgent.velocity;
        }

        public override void Force(Vector3 force) { }

        public override void Material(bool friction) { }

        public override void Exit()
        {
            // Set Movement Parameters
            _navMeshAgent.speed = 0;
            _navMeshAgent.acceleration = 0;
            _navMeshAgent.SetDestination(RootTransform.position);
        }
    }
}