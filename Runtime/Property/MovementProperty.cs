using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Property/Movement Property")]
    public sealed class MovementProperty : Property
    {
        [Range(0, 1)] public float WalkScale = 1.0f;
        [Range(0, 1)] public float RunScale = 1.0f;
        [Range(0, 1)] public float JumpScale = 1.0f;
        [Range(1, 10)] public int Rate = 10;

        // Jump Fields
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        private bool _isLevitation = false;

        // Model Components
        private Inputable _inputable;
        private Animatorable _animatorable;
        private Movable _movable;
        private Positionable _positionable;

        // Property Methods
        public override void OnEnableState()
        {
            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
            _animatorable = AddComponentInRoot<Animatorable>();
            _positionable = AddComponentInRoot<Positionable>();
            _movable = AddComponentInRoot<Movable>();

            _movable.Enable();
        }

        public override void OnEnterState()
        {
            _movable.Enter();
        }

        public override void OnFixedActiveState()
        {
            // Set Movement Parameters    
            float speed = _inputable.ShiftState == ButtonState.Down ? RunScale * _movable.RunSpeed : WalkScale * _movable.WalkSpeed;

            _movable.Horizontal(_positionable.GetDirection(_inputable.MoveVector), speed, Rate);

            // Set Animation Parameters
            _animatorable.Speed = _movable.Velocity.magnitude;
            _animatorable.Grounded = _positionable.IsGrounded;
        }

        public override void OnActiveState()
        {
            jumpLoop();

            _movable.Material(_positionable.IsGrounded && _positionable.IsObstacle == false);
        }

        public override void OnExitState()
        {
            // Set Movement Parameters 
            _movable.Exit();

            setLevitation(false);
        }

        private void jumpLoop()
        {
            if (JumpScale > 0)
            {
                // Input Jump
                _isJumpPressed = _inputable.MotionState == ButtonState.Down;

                setLevitation(_isJumpPressed);

                if (_isJumpPressed == false)
                {
                    if (_positionable.IsGrounded == true)
                    {
                        _isJumpDone = false;
                        _movable.JumpCounter = _movable.ExtraJumps;
                    }
                    else
                    {

                        if (_movable.JumpCounter > 0)
                        {
                            _isJumpDone = false;
                        }
                    }
                }

                // Force Update
                if (_isJumpDone == false)
                {
                    if (_isJumpPressed == true)
                    {
                        _movable.Force(JumpScale * Vector3.up * _movable.JumpHeight.HeightToForce(_movable.Gravity));

                        if (_positionable.IsGrounded == false)
                        {
                            _movable.JumpCounter--;
                        }

                        _isJumpDone = true;
                    }
                }
            }
        }

        private void setLevitation(bool state)
        {
            if (state == true)
            {
                if (_isLevitation == false)
                {
                    _movable.Gravity = _movable.Gravity - _movable.Levitation;
                    _isLevitation = true;
                }
            }
            else
            {
                if (_isLevitation == true)
                {
                    _movable.Gravity = _movable.Gravity + _movable.Levitation;
                    _isLevitation = false;
                }
            }
        }
    }
}