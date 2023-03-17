using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionMovement : Action
    {
        public float MoveSpeed = 3;
        public float MoveShift = 5;
        public float JumpForce = 5;

        //public float Levitation = 0.5f;

        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop() 
        {
            JumpInput();
        }

        public override void FixedLoop()
        {
            MoveHandler();
            JumpHandler();
        }

        public override void Exit() => movable.FreezAll();

        protected void MoveHandler()
        {
            Vector3 direction = new Vector3(input.MoveHorizontal, 0, input.MoveVertical);
            float speed = input.Shift ? MoveShift : MoveSpeed;

            animatorable.Play(Name, (direction * speed).magnitude);
            movable.MoveToDirection(direction, speed);
        }

        protected void JumpHandler()
        {
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    movable.Jump(JumpForce);
                    _isJumpDone = true;
                }
            }
        }

        protected void JumpInput()
        {
            _isJumpPressed = input.Motion;

            if (_isJumpPressed == false)
            {
                if (positionable)
                {
                    if (positionable.IsGrounded)
                    {
                        _isJumpDone = false;
                    }
                }
                else
                {
                    _isJumpDone = false;
                }
            }
        }
    }
}