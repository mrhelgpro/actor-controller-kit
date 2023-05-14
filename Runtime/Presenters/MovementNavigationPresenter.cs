using UnityEngine.AI;
using UnityEngine;

namespace Actormachine
{
    public sealed class MovementNavigationPresenter : Presenter
    {
        [Range(1, 5)] public float MoveSpeed = 3f;
        [Range(1, 10)] public float MoveShift = 5f;
        [Range(1, 10)] public int Rate = 10;

        // Move Fields
        private Vector3 _currentDirection = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _lerpDirection = Vector3.zero;
        private float _currentSpeed = 0;

        // Model Components
        private Inputable _inputable;
        private Animatorable _animatorable;
        private Movable _movable;
        private Positionable _positionable;

        // Unity Components
        private NavMeshAgent _navMeshAgent;

        public override void Initiation()
        {
            // Get components using "GetComponentInRoot" to create them on <Actor>
            _inputable = GetComponentInRoot<Inputable>();
            _animatorable = GetComponentInRoot<Animatorable>();
            _movable = GetComponentInRoot<Movable>();
            _positionable = GetComponentInRoot<Positionable>();

            _navMeshAgent = GetComponentInRoot<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
        }

        public override void Enter()
        {
            _currentVelocity = Vector3.zero;
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

        public override void UpdateLoop()
        {
            float maxSpeed = _inputable.ShiftState ? MoveShift : MoveSpeed;

            _currentSpeed = _movable.GetSpeed(maxSpeed);
            _currentDirection = _positionable.GetDirection(_inputable.MoveVector);

            _lerpDirection = Vector3.Lerp(_lerpDirection, _currentDirection, Time.deltaTime * Rate);
            _currentVelocity = new Vector3(_lerpDirection.x, _currentDirection.y, _lerpDirection.z) * _currentSpeed;

            _animatorable.Play(_positionable.IsGrounded ? StateName : "Fall");
            _animatorable.SetFloat("Speed", _currentVelocity.magnitude);
        }

        public override void FixedUpdateLoop()
        {
            _navMeshAgent.speed = _currentSpeed;
            _navMeshAgent.acceleration = Rate * 2;
            _navMeshAgent.SetDestination(RootTransform.position + _currentDirection.normalized);
        }

        public override void Exit()
        {
            _currentVelocity = Vector3.zero;

            _navMeshAgent.speed = 0;
            _navMeshAgent.acceleration = 0;
            _navMeshAgent.SetDestination(RootTransform.position);
        }
    }
}