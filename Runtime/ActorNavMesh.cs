using UnityEngine.AI;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActorNavMesh : Actor
    {
        public void AddComponents() 
        {
            gameObject.AddThisComponent<Inputable>();
            gameObject.AddThisComponent<Movable>();
            gameObject.AddThisComponent<Positionable>();

            NavMeshAgent navMeshAgent = gameObject.AddThisComponent<NavMeshAgent>();
            navMeshAgent.agentTypeID = 0;
            navMeshAgent.baseOffset = 0;
            navMeshAgent.speed = 2f;
            navMeshAgent.angularSpeed = 10000;
            navMeshAgent.acceleration = 100;
            navMeshAgent.stoppingDistance = 0;
            navMeshAgent.autoBraking = true;
            navMeshAgent.radius = 0.25f;
            navMeshAgent.height = navMeshAgent.radius * 2;
            navMeshAgent.avoidancePriority = 50;
            navMeshAgent.autoTraverseOffMeshLink = true;
            navMeshAgent.autoRepath = true;
        }

        public void RemoveComponents()
        {
            gameObject.RemoveComponent<Inputable>();
            gameObject.RemoveComponent<Movable>();
            gameObject.RemoveComponent<Positionable>();
            gameObject.RemoveComponent<NavMeshAgent>();
        }
    }
}