using UnityEngine.AI;
using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Property/MovementNavigation Property")]
    public sealed class MovementNavigationProperty : Property
    {
        [Range(0, 1)] public float WalkScale = 1.0f;
        [Range(0, 1)] public float RunScale = 1.0f;
        [Range(1, 10)] public int Rate = 10;

        // Move Fields
        private Vector3 _currentDirection = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;

        // Model Components
        private Inputable _inputable;
        private Animatorable _animatorable;
        private Movable _movable;
        private Positionable _positionable;

        // Unity Components
        private NavMeshAgent _navMeshAgent;

        private Transform _rootTransform;

        // Property Methods
        public override void OnEnterState()
        {
            _rootTransform = FindRootTransform;

            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
            _animatorable = AddComponentInRoot<Animatorable>();
            _movable = AddComponentInRoot<Movable>();
            _positionable = AddComponentInRoot<Positionable>();
            _navMeshAgent = AddComponentInRoot<NavMeshAgent>();

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

        public override void OnFixedActiveState()
        {
            // Set Movement Parameters 
            float speed = _inputable.ShiftState ? RunScale * _movable.RunSpeed : WalkScale * _movable.WalkSpeed;

            _currentDirection = _positionable.GetDirection(_inputable.MoveVector);

            _navMeshAgent.speed = speed;
            _navMeshAgent.acceleration = Rate * 2;
            _navMeshAgent.SetDestination(_rootTransform.position + _currentDirection.normalized);

            // Set Animation Parameters
            _animatorable.Speed = _currentVelocity.magnitude;
            _animatorable.Grounded = _positionable.IsGrounded;
        }

        public override void OnExitState()
        {
            // Set Movement Parameters
            _currentDirection = Vector3.zero;
            _currentVelocity = Vector3.zero;

            _navMeshAgent.speed = 0;
            _navMeshAgent.acceleration = 0;
            _navMeshAgent.SetDestination(_rootTransform.position);

            // Set Animation Parameters
            _animatorable.Speed = 1;
            _animatorable.Grounded = true;
        }
    }
}