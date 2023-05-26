using UnityEngine.AI;
using UnityEngine;

namespace Actormachine
{
    public sealed class MovementNavigationPresenter : StateBehaviour, IEnterState, IFixedActiveState, IExitState
    {
        [Range(1, 5)] public float MoveSpeed = 3f;
        [Range(1, 10)] public float MoveShift = 5f;
        [Range(1, 10)] public int Rate = 10;

        // Move Fields
        private Vector3 _currentDirection = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;
        private float _currentSpeed = 0;

        // Model Components
        private Inputable _inputable;
        private Animatorable _animatorable;
        private Movable _movable;
        private Positionable _positionable;

        // Unity Components
        private NavMeshAgent _navMeshAgent;

        private Transform _rootTransform;

        // Presenter Methods
        public void OnEnterState()
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

        public void OnFixedActiveState()
        {
            // Set Movement Parameters 
            float maxSpeed = _inputable.ShiftState ? MoveShift : MoveSpeed;

            _currentSpeed = _movable.GetSpeed(maxSpeed);
            _currentDirection = _positionable.GetDirection(_inputable.MoveVector);

            _navMeshAgent.speed = _currentSpeed;
            _navMeshAgent.acceleration = Rate * 2;
            _navMeshAgent.SetDestination(_rootTransform.position + _currentDirection.normalized);

            // Set Animation Parameters
            _animatorable.Speed = _currentVelocity.magnitude;
            _animatorable.Grounded = _positionable.IsGrounded;
        }

        public void OnExitState()
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