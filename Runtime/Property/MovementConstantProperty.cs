using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Property/MovementConstant Property")]
    public class MovementConstantProperty : Property
    {
        public float Force = 1;

        // Move Fields
        private Vector3 _currentDirection = Vector3.zero;

        // Model Components
        private Inputable _inputable;
        private Movable _movable;
        private Positionable _positionable;

        public override void OnEnableState()
        {
            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
            _positionable = AddComponentInRoot<Positionable>();
            _movable = AddComponentInRoot<Movable>();

            _movable.Enable();
        }

        public override void OnEnterState()
        {
            _movable.Enter();
            _movable.Material(false);

            // Set Value
            _currentDirection = _inputable.MoveVector.magnitude > 0 ? _positionable.GetDirection(_inputable.MoveVector).normalized : RootTransform.TransformDirection(Vector3.forward).normalized;
        }

        public override void OnFixedActiveState()
        {
            // Set Movement Parameters    
            _movable.Horizontal(_currentDirection, Force, 100);
        }

        public override void OnExitState()
        {
            _movable.Exit();
        }
    }
}